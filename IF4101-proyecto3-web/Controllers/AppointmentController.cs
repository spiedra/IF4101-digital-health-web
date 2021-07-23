using IF4101_proyecto3_web.Data;
using IF4101_proyecto3_web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace IF4101_proyecto3_web.Controllers
{
    public class AppointmentController : Controller
    {

        [Route("Appointment/Schedule")]
        public IActionResult AppointmentRegister(string message)
        {
            ConnectionDb connectionDb = new();
            this.GetListsToCbx(connectionDb);
            ViewBag.response = message;
            return View();
        }

        [Route("Appointment/Manage")]
        public IActionResult AppointmentManage()
        {
            return View();
        }

        [HttpPost]
        [Route("Appointment/Manage")]
        public JsonResult AppointmentManage(string PaitentCardId)
        {
            if (!string.IsNullOrEmpty(PaitentCardId))
            {
                ConnectionDb connectionDb = new();
                return Json(ExcGetAppointmetsByCard(connectionDb, PaitentCardId));
            }
            return Json(null);
        }

        [HttpPost]
        [Route("Appointment/ManageDelete")]
        public JsonResult AppointmentManage(int AppointmentId)
        {
            ConnectionDb connectionDb = new ConnectionDb();
            this.ExcDeleteAppointment(connectionDb, AppointmentId);
            return Json("Successfully removed");
        }

        [HttpPost]
        [Route("Appointment/ManageDate")]
        public JsonResult AppointmentManageDate(int AppointmentId)
        {
            ConnectionDb connectionDb = new ConnectionDb();
            return Json(this.ExcValidateAppointmentDate(connectionDb, AppointmentId));
        }

        [HttpPost]
        [Route("Appointment/ManageDiagnostic")]
        public JsonResult AppointmentManage(int AppointmentId, string DiagnosticDescription)
        {
            ConnectionDb connectionDb = new ConnectionDb();
            this.ExcAddAppointmentDescription(connectionDb, AppointmentId, DiagnosticDescription);
            return Json("Successfully updated");
        }

        [HttpPost]
        [Route("Appointment/Schedule_")]
        public IActionResult AppointmentRegisterParam(AppointmentViewModel appointmentViewModel)
        {
            ConnectionDb connectionDb = new();
            this.GetListsToCbx(connectionDb);
            if (ExcRegisterAppointment(connectionDb, appointmentViewModel))
            {
                return RedirectToAction(nameof(AppointmentRegister), new { message = "Successful registration" });
            }
            return RedirectToAction(nameof(AppointmentRegister), new { message = "It could not be completed. Please try again" });
        }

        [HttpGet]
        public JsonResult GetListMedicalCenters()
        {
            ConnectionDb connectionDb = new ConnectionDb();
            return Json(this.ExcGetList(connectionDb, "ADMINISTRATOR.sp_GET_MEDICALS_CENTERS_NAMES"));
        }

        [HttpGet]
        public JsonResult GetListSpecialtyTypes()
        {
            ConnectionDb connectionDb = new ConnectionDb();
            return Json(this.ExcGetList(connectionDb, "ADMINISTRATOR.sp_GET_SPECIALTIES_TYPES"));
        }

        [HttpPost]
        public JsonResult ManageUpdate(AppointmentViewModel appointmentViewModel)
        {
            ConnectionDb connectionDb = new ConnectionDb();
            this.ExcUpdateAppointment(connectionDb, appointmentViewModel);
            return Json("Successfully updated");
        }

        private void GetListsToCbx(ConnectionDb connectionDb)
        {
            ViewBag.HealthCenters = this.ExcGetList(connectionDb, "ADMINISTRATOR.sp_GET_MEDICALS_CENTERS_NAMES");
            ViewBag.SpecialityTypes = this.ExcGetList(connectionDb, "ADMINISTRATOR.sp_GET_SPECIALTIES_TYPES");
        }

        private List<string> ExcGetList(ConnectionDb connectionDb, string commandText)
        {
            connectionDb.InitSqlComponents(commandText);
            connectionDb.ExcecuteReader();
            return ReadGetList(connectionDb);
        }

        private List<string> ReadGetList(ConnectionDb connectionDb)
        {
            List<string> list = new List<string>();
            while (connectionDb.SqlDataReader.Read())
            {
                list.Add(connectionDb.SqlDataReader.GetString(0));
            }
            connectionDb.SqlConnection.Close();
            return list;
        }

        private bool ExcRegisterAppointment(ConnectionDb connectionDb, AppointmentViewModel appointmentViewModel)
        {
            string paramIdCard = "@param_ID_CARD"
           , paramMedicalCenter = "@param_MEDICAL_CENTER_NAME"
           , paramDate = "@param_DATE"
           , paramSpecialityType = "@param_SPECIALTY_TYPE"
           , commandText = "ADMINISTRATOR.sp_REGISTER_APPOINTMENT";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramIdCard, SqlDbType.VarChar, appointmentViewModel.PatientCard);
            connectionDb.CreateParameter(paramMedicalCenter, SqlDbType.VarChar, appointmentViewModel.HealthCenter);
            connectionDb.CreateParameter(paramDate, SqlDbType.DateTime, appointmentViewModel.Date);
            connectionDb.CreateParameter(paramSpecialityType, SqlDbType.VarChar, appointmentViewModel.SpecialityType);
            connectionDb.CreateParameterOutput();
            connectionDb.ExcecuteReader();
            return this.ReadValidateLogIn(connectionDb);
        }

        private bool ReadValidateLogIn(ConnectionDb connectionDb)
        {
            if ((int)connectionDb.ParameterReturn.Value == 1)
                return true;

            connectionDb.SqlConnection.Close();
            return false;
        }

        private List<AppointmentViewModel> ExcGetAppointmetsByCard(ConnectionDb connectionDb, string PaitentCardId)
        {
            string paramIdCard = "@param_ID_CARD"
          , commandText = "ADMINISTRATOR.sp_GET_APPOINTMENTS_BY_CARD";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramIdCard, SqlDbType.VarChar, PaitentCardId);
            connectionDb.ExcecuteReader();
            return this.ReadGetAppointmetsByCard(connectionDb);
        }

        private List<AppointmentViewModel> ReadGetAppointmetsByCard(ConnectionDb connectionDb)
        {
            List<AppointmentViewModel> list = new List<AppointmentViewModel>();
            while (connectionDb.SqlDataReader.Read())
            {
                list.Add(new()
                {
                    PatientName = connectionDb.SqlDataReader.GetString(0),
                    Date = connectionDb.SqlDataReader.GetDateTime(1),
                    HealthCenter = connectionDb.SqlDataReader.GetString(2),
                    SpecialityType = connectionDb.SqlDataReader.GetString(3),
                    AppointmentId = connectionDb.SqlDataReader.GetInt32(4),
                    Description = this.CheckIsNull(connectionDb)
                });
            }
            connectionDb.SqlConnection.Close();
            return list;
        }

        private void ExcDeleteAppointment(ConnectionDb connectionDb, int AppointmentId)
        {
            string paramIdCard = "@param_APPOINTMENT_ID"
          , commandText = "ADMINISTRATOR.sp_DELETE_APPOINTMENT";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramIdCard, SqlDbType.Int, AppointmentId);
            connectionDb.ExecuteNonQuery();
        }

        private string CheckIsNull(ConnectionDb connectionDb)
        {
            if (!connectionDb.SqlDataReader.IsDBNull(5))
            {
                return connectionDb.SqlDataReader.GetString(5);
            }
            return "Pending appointment";
        }

        private void ExcAddAppointmentDescription(ConnectionDb connectionDb, int AppointmentId, string DiagnosticDescription)
        {
            string paramAppointmentId = "@param_APPOINTMENT_ID"
          , paramDiagnosticDescription = "@param_DESCRIPTION"
          , commandText = "ADMINISTRATOR.sp_ADD_APPOINTMENT_DESCRIPTION";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramAppointmentId, SqlDbType.Int, AppointmentId);
            connectionDb.CreateParameter(paramDiagnosticDescription, SqlDbType.VarChar, DiagnosticDescription);
            connectionDb.ExecuteNonQuery();
        }

        private int ExcValidateAppointmentDate(ConnectionDb connectionDb, int AppointmentId)
        {
            string paramAppointmentId = "@param_APPOINTMENT_ID"
          , commandText = "ADMINISTRATOR.sp_VALIDATE_APPOINTMENT_DATE";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramAppointmentId, SqlDbType.Int, AppointmentId);
            connectionDb.CreateParameterOutput();
            connectionDb.ExcecuteReader();
            return (int)connectionDb.ParameterReturn.Value;
        }

        private void ExcUpdateAppointment(ConnectionDb connectionDb, AppointmentViewModel appointmentViewModel)
        {
            string paramAppointmentId = "@param_APPOINTMENT_ID"
           , paramMedicalCenter = "@param_MEDICAL_CENTER_NAME"
           , paramDate = "@param_DATE"
           , paramSpecialityType = "@param_SPECIALTY_TYPE"
           , commandText = "ADMINISTRATOR.sp_UPDATE_APPOINTMENT";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramAppointmentId, SqlDbType.Int, appointmentViewModel.AppointmentId);
            connectionDb.CreateParameter(paramMedicalCenter, SqlDbType.VarChar, appointmentViewModel.HealthCenter);
            connectionDb.CreateParameter(paramDate, SqlDbType.DateTime, appointmentViewModel.Date);
            connectionDb.CreateParameter(paramSpecialityType, SqlDbType.VarChar, appointmentViewModel.SpecialityType);
            connectionDb.ExecuteNonQuery();
        }
    }
}

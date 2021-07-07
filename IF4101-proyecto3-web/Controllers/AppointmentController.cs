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
        public IActionResult AppointmentRegister()
        {
            ConnectionDb connectionDb = new();
            this.GetListsToCbx(connectionDb);
            return View();
        }

        [HttpPost]
        [Route("Appointment/Schedule")]
        public IActionResult AppointmentRegister(AppointmentViewModel appointmentViewModel)
        {
            ConnectionDb connectionDb = new ConnectionDb();
            this.GetListsToCbx(connectionDb);
            if (ExcRegisterAppointment(connectionDb, appointmentViewModel))
            {
                ViewBag.response = "Successful registration";
                return View();
            }
            ViewBag.response = "Appointment is already scheduled";
            return View();
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
    }
}

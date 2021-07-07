﻿using IF4101_proyecto3_web.Data;
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

        [Route("Appointment/Manage")]
        public IActionResult AppointmentManage()
        {
            return View();
        }

        [HttpPost]
        [Route("Appointment/Manage")]
        public JsonResult AppointmentManage(string PaitentCardId)
        {
            ConnectionDb connectionDb = new ConnectionDb();
            return Json(this.ExcGetAppointmetsByCard(connectionDb, PaitentCardId));
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
                    AppointmentId = connectionDb.SqlDataReader.GetInt32(4)
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
    }
}

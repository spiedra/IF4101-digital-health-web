using IF4101_proyecto3_web.Data;
using IF4101_proyecto3_web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IF4101_proyecto3_web.Controllers
{
    public class AppointmentController : Controller
    {

        [Route("Appointment/Schedule")]
        public IActionResult AppointmentRegister()
        {
            ConnectionDb connectionDb = new ConnectionDb();
            ViewBag.HealthCenters = this.ExcGetList(connectionDb, "ADMINISTRATOR.sp_GET_MEDICALS_CENTERS_NAMES");
            ViewBag.SpecialityTypes = this.ExcGetList(connectionDb, "ADMINISTRATOR.sp_GET_SPECIALTIES_TYPES");
            return View();
        }

        [HttpPost]
        [Route("Appointment/Schedule")]
        public IActionResult AppointmentRegister(AppointmentViewModel appointmentViewModel)
        {
            // aca se pone si se ingreso o si no se ingreso
            return View();
        }

        private void ExcGetSpecialtiesTypes(ConnectionDb connectionDb)
        {
            string commandText = "ADMINISTRATOR.sp_GET_SPECIALTIES_TYPES";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.ExcecuteReader();
        }

        private List<string> ReadGetSpecialitiesTypes(ConnectionDb connectionDb)
        {
            List<string> list = new List<string>();
            while (connectionDb.SqlDataReader.Read())
            {
                list.Add(connectionDb.SqlDataReader.GetString(0));
            }
            connectionDb.SqlConnection.Close();
            return list;
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
    }
}

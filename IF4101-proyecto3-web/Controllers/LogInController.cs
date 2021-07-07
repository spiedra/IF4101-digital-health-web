using IF4101_proyecto3_web.Data;
using IF4101_proyecto3_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace IF4101_proyecto3_web.Controllers
{
    public class LogInController : Controller
    {
        public IConfiguration Configuration { get; }

        [HttpPost]
        public JsonResult ValidateInputLogIn(DoctorViewModel doctor)
        {
            if (ModelState.IsValid)
            {
                ConnectionDb connectionDb = new ConnectionDb();
                this.ExcValidateLogIn(connectionDb, doctor);
                if (this.ReadValidateLogIn(connectionDb))
                {
<<<<<<< HEAD

                    return null;
=======
                    return null ;
>>>>>>> 3044ef8d2af934733ce3e0937759f1b69ca7ed04
                }
            }
            return null;
        }

        private void ExcValidateLogIn(ConnectionDb connectionDb, DoctorViewModel doctor)
        {
            string paramId = "@param_ID_CARD"
             , paramDoctorCode = "@param_DOCTOR_CODE"
             , paramPassword = "@param_PASSWORD"
             , commandText = "ADMINISTRATOR.sp_VALIDATE_DOCTOR_LOG_IN";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramId, SqlDbType.VarChar, doctor.IdCard);
            connectionDb.CreateParameter(paramDoctorCode, SqlDbType.VarChar, doctor.DoctorCode);
            connectionDb.CreateParameter(paramPassword, SqlDbType.VarChar, doctor.Password);
            connectionDb.CreateParameterOutput();
            connectionDb.ExcecuteReader();
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

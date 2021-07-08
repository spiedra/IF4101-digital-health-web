using IF4101_proyecto3_web.Data;
using IF4101_proyecto3_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IF4101_proyecto3_web.Controllers
{
    public class IndexController : Controller
    {
        private readonly ILogger<IndexController> _logger;

        public IndexController(ILogger<IndexController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.ShowModalResponse = false;
            return View();
        }

        [Route("Index/Administration")]
        public IActionResult MainAdmin()
        {
            return View();
        }

        [HttpPost]
        [Route("Index/LogIn")]
        public IActionResult ValidateInputLogIn(DoctorViewModel doctor)
        {
            if (ModelState.IsValid)
            {
                ConnectionDb connectionDb = new ConnectionDb();
                this.ExcValidateLogIn(connectionDb, doctor);
                if (this.ReadValidateLogIn(connectionDb))
                {
                    ViewBag.ShowModalResponse = false;
                    return View("MainAdmin");
                }
            }
            ViewBag.ShowModalResponse = true;
            return View("Index");
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

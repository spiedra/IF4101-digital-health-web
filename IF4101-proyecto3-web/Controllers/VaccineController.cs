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
    public class VaccineController : Controller
    {
        public IActionResult RegisterVaccination()
        {

            ConnectionDb connectionDb = new ConnectionDb();
            string commandText = "ADMINISTRATOR.sp_LIST_VACCINE";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.ExcecuteReader();

            ViewBag.Vaccines = this.GetVaccines(connectionDb);
            connectionDb.SqlConnection.Close();
            return View();
        }

        public List<string> GetVaccines(ConnectionDb connectionDb)
        {
            List<string> vaccines = new List<string>();
            while (connectionDb.SqlDataReader.Read())
            {
                vaccines.Add(connectionDb.SqlDataReader["VACCINE_TYPE"].ToString());
            }
            return vaccines;
        }

        [HttpPost]

        public string RegisterVaccination(VaccinationViewModel model)
        {
            ConnectionDb connectionDb = new ConnectionDb();
            this.ExcRegisterVaccination(connectionDb, model);
            if (this.ReadValidateRegister(connectionDb))
            {
                return "1"; //se ha podido registrar la vacuna
            }
            else
            {
                return "-1"; //el paciente no existe
            }
        }
        private void ExcRegisterVaccination(ConnectionDb connectionDb, VaccinationViewModel model)
        {
            string paramId = "@param_ID_CARD"
         , paramVaccineType = "@param_VACCINE_TYPE"
          , paramDescription = "@param_DESCRIPTION"
          , paramLattestVaccineDate = "@param_LATTEST_VACCINE_DATE"
          , paramNextVaccineDate = "@param_NEXT_VACCINE_DATE"
          , commandText = "ADMINISTRATOR.sp_REGISTER_VACCINATION";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramId, SqlDbType.VarChar, model.IdCard);
            connectionDb.CreateParameter(paramVaccineType, SqlDbType.VarChar, model.VaccinationType);
            connectionDb.CreateParameter(paramDescription, SqlDbType.VarChar, model.Description);
            connectionDb.CreateParameter(paramLattestVaccineDate, SqlDbType.Date, model.ApplicationDate);
            connectionDb.CreateParameter(paramNextVaccineDate, SqlDbType.Date, model.NextVaccinationDate);

            connectionDb.CreateParameterOutput();
            connectionDb.ExcecuteReader();
        }

        private bool ReadValidateRegister(ConnectionDb connectionDb)
        {
            if ((int)connectionDb.ParameterReturn.Value == 1)
                return true;

            connectionDb.SqlConnection.Close();
            return false;
        }

        public IActionResult ManageVaccine()
        {
            ConnectionDb connectionDb = new ConnectionDb();
            string commandText = "ADMINISTRATOR.sp_LIST_VACCINE";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.ExcecuteReader();

            ViewBag.Vaccines = this.GetVaccines(connectionDb);
            return View();
        }

        [HttpGet]

        public JsonResult ListPatientVaccine(string IdCard)
        {
            ConnectionDb connectionDb = new ConnectionDb();
            this.ExcListVaccinePatient(connectionDb, IdCard);
            List<VaccinationViewModel> vaccines = this.GetPatientVaccination(connectionDb);
            connectionDb.SqlConnection.Close();
            return Json(vaccines);
        }

        private void ExcListVaccinePatient(ConnectionDb connectionDb, string IdCard)
        {
            string paramId = "@param_ID_CARD", commandText = "ADMINISTRATOR.sp_LIST_PATIENT_VACCINE";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramId, SqlDbType.VarChar, IdCard);
            connectionDb.ExcecuteReader();
        }

        public List<VaccinationViewModel> GetPatientVaccination(ConnectionDb connectionDb)
        {
            List<VaccinationViewModel> vaccines = new List<VaccinationViewModel>();
            while (connectionDb.SqlDataReader.Read())
            {
                VaccinationViewModel model = new VaccinationViewModel();
                model.VaccinationType = connectionDb.SqlDataReader["VACCINE_TYPE"].ToString();
                model.FullName = connectionDb.SqlDataReader["full_name"].ToString();
                model.Description = connectionDb.SqlDataReader["DESCRIPTION"].ToString();
                model.ApplicationDate = connectionDb.SqlDataReader["LATTEST_VACCINE_DATE"].ToString();
                model.NextVaccinationDate = connectionDb.SqlDataReader["NEXT_VACCINE_DATE"].ToString();
                vaccines.Add(model);
            }
            return vaccines;
        }

        [HttpDelete]
        public string DeletePatientVaccine(string IdCard,string VaccinationType)
        {
            ConnectionDb connectionDb = new ConnectionDb();
            string paramId = "@param_ID_CARD", paramVaccineType= "@param_VACCINATION_TYPE", commandText = "Patient.sp_DELETE_PATIENT_VACCINE";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramId, SqlDbType.VarChar, IdCard);
            connectionDb.CreateParameter(paramVaccineType, SqlDbType.VarChar, VaccinationType);
            connectionDb.ExcecuteReader();
            connectionDb.SqlConnection.Close();
            return "1";
        }

        [HttpPut]
        public string UpdatePatientVaccine(string IdCard, string OldVaccineType, string Vaccinetype, string Description, string ApplicationDate, string NextVaccinationDate)
        {
            ConnectionDb connectionDb = new ConnectionDb();
            string paramId = "@param_ID_CARD"
           , paramOldVaccineType = "@param_OLD_VACCINATION_TYPE"
           , paramVaccineType = "@param_VACCINATION_TYPE"
           , paramDescription = "@param_DESCRIPTION"
           , paramLattestVaccineDate = "@param_LATTEST_VACCINE_DATE"
           , paramNextVaccineDate = "@param_NEXT_VACCINE_DATE"
           ,commandText = "PATIENT.sp_UPDATE_PATIENT_VACCINE";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramId, SqlDbType.VarChar, IdCard);
            connectionDb.CreateParameter(paramOldVaccineType, SqlDbType.VarChar, OldVaccineType);
            connectionDb.CreateParameter(paramVaccineType, SqlDbType.VarChar, Vaccinetype);
            connectionDb.CreateParameter(paramDescription, SqlDbType.VarChar, Description);
            connectionDb.CreateParameter(paramLattestVaccineDate, SqlDbType.VarChar, ApplicationDate);
            connectionDb.CreateParameter(paramNextVaccineDate, SqlDbType.VarChar, NextVaccinationDate);
            connectionDb.ExcecuteReader();
            connectionDb.SqlConnection.Close();
            return "1";
        }

        
    }
}

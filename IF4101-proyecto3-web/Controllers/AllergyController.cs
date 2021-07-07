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
    public class AllergyController : Controller
    {
        public IActionResult RegisterAllergy()
        {
            
            ViewBag.Allergies = this.RequestAllergies();
            return View();
        }

        public List<string> RequestAllergies()
        {
            ConnectionDb connectionDb = new ConnectionDb();
            string commandText = "ADMINISTRATOR.sp_LIST_ALLERGY";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.ExcecuteReader();

            return this.GetAllergies(connectionDb);
        }
        public List<string> GetAllergies(ConnectionDb connectionDb)
        {
            List<string> allergies = new List<string>();
            while (connectionDb.SqlDataReader.Read())
            {
                allergies.Add(connectionDb.SqlDataReader["ALLERGY_TYPE"].ToString());
            }
            connectionDb.SqlConnection.Close();
            return allergies;
        }

        public List<string> RequestMedicaments()
        {
            ConnectionDb connectionDb = new ConnectionDb();
            string commandText = "ADMINISTRATOR.sp_LIST_MEDICINE";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.ExcecuteReader();

            return this.GetMedicaments(connectionDb);
        }

        public List<string> GetMedicaments(ConnectionDb connectionDb)
        {
            List<string> medicine = new List<string>();
            while (connectionDb.SqlDataReader.Read())
            {
               medicine.Add(connectionDb.SqlDataReader["MEDICINE_TYPE"].ToString());
            }
            connectionDb.SqlConnection.Close();
            return medicine;
        }

        public IActionResult ManageAllergy()
        {
            ViewBag.Allergies = this.RequestAllergies();
            ViewBag.Medicaments = this.RequestMedicaments();
            return View();
        }

        [HttpPost]
        public string RegisterAllergy(AllergyViewModel model)
        {
            ConnectionDb connectionDb = new ConnectionDb();
            this.excRegisterAllergy(connectionDb, model);
            var resp = ReadValidateRegister(connectionDb);
            connectionDb.SqlConnection.Close();
            return resp.ToString();
        }

        public void excRegisterAllergy(ConnectionDb connectionDb, AllergyViewModel model)
        {
            string paramId = "@param_ID_CARD"
                     , paramAllergyType = "@param_ALLERGY_TYPE"
                      , paramDescription = "@param_DESCRIPTION"
                      , paramDiagnosticDate = "@param_DIAGNOSTIC_DATE"
                      , commandText = "PATIENT.sp_REGISTER_PATIENT_ALLERGY";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramId, SqlDbType.VarChar, model.IdCard);
            connectionDb.CreateParameter(paramAllergyType, SqlDbType.VarChar, model.AllergyType);
            connectionDb.CreateParameter(paramDescription, SqlDbType.VarChar, model.Description);
            connectionDb.CreateParameter(paramDiagnosticDate, SqlDbType.VarChar, model.DiagnosticDate);

            connectionDb.CreateParameterOutput();
            connectionDb.ExcecuteReader();
        }

        private int ReadValidateRegister(ConnectionDb connectionDb)
        {
            return (int)connectionDb.ParameterReturn.Value;
        }

        [HttpGet]
        public JsonResult ListPatientAllergies(string IdCard)
        {
            ConnectionDb connectionDb = new ConnectionDb();
            this.ExcListPatientAllergies(connectionDb, IdCard);
            List<AllergyViewModel> allergies = this.GetPatientAllergies(connectionDb);
            connectionDb.SqlConnection.Close();
            return Json(allergies);
        }

        private void ExcListPatientAllergies(ConnectionDb connectionDb, string IdCard)
        {
            string paramId = "@param_ID_CARD", commandText = "PATIENT.sp_LIST_PATIENT_ALLERGIES";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramId, SqlDbType.VarChar, IdCard);
            connectionDb.ExcecuteReader();
        }
        public List<AllergyViewModel> GetPatientAllergies(ConnectionDb connectionDb)
        {
            List<AllergyViewModel> allergies = new List<AllergyViewModel>();
            while (connectionDb.SqlDataReader.Read())
            {
                AllergyViewModel model = new AllergyViewModel();
                model.AllergyType = connectionDb.SqlDataReader["ALLERGY_TYPE"].ToString();
                model.FullName = connectionDb.SqlDataReader["full_name"].ToString();
                model.Description = connectionDb.SqlDataReader["DESCRIPTION"].ToString();
                model.DiagnosticDate = connectionDb.SqlDataReader["DIAGNOSTIC_DATE"].ToString();
             
                allergies.Add(model);
            }
            return allergies;
        }

        [HttpDelete]
        public string DeletePatientAllergy(string IdCard, string AllergyType)
        {
            ConnectionDb connectionDb = new ConnectionDb();
            string paramId = "@param_ID_CARD", paramAllergyType = "@param_ALLERGY_TYPE", commandText = "PATIENT.sp_DELETE_PATIENT_ALERGY";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramId, SqlDbType.VarChar, IdCard);
            connectionDb.CreateParameter(paramAllergyType, SqlDbType.VarChar, AllergyType);
            connectionDb.ExcecuteReader();
            connectionDb.SqlConnection.Close();
            return "1";
        }

        [HttpPut]
        public string UpdatePatientAllergy(string IdCard, string OldAllergyType, string AllergyType, string Description, string DiagnosticDate )
        {
            ConnectionDb connectionDb = new ConnectionDb();
            string paramId = "@param_ID_CARD"
           , paramOldAllergyType = "@param_OLD_ALLERGY_TYPE"
           , paramAllergyType = "@param_ALLERGY_TYPE"
           , paramDescription = "@param_DESCRIPTION"
           , paramDiagnosticDate = "@param_DIAGNOSTIC_DATE"
           , commandText = "PATIENT.sp_UPDATE_PATIENT_ALLERGY";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramId, SqlDbType.VarChar, IdCard);
            connectionDb.CreateParameter(paramOldAllergyType, SqlDbType.VarChar, OldAllergyType);
            connectionDb.CreateParameter(paramAllergyType, SqlDbType.VarChar, AllergyType);
            connectionDb.CreateParameter(paramDescription, SqlDbType.VarChar, Description);
            connectionDb.CreateParameter(paramDiagnosticDate, SqlDbType.VarChar, DiagnosticDate);
            connectionDb.ExcecuteReader();
            connectionDb.SqlConnection.Close();
            return "1";
        }
    }
}

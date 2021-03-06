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
            ConnectionDb connectionDb = new();
            string commandText = "ADMINISTRATOR.sp_LIST_ALLERGY";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.ExcecuteReader();

            return this.GetAllergies(connectionDb);
        }
        public List<string> GetAllergies(ConnectionDb connectionDb)
        {
            List<string> allergies = new();
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
            ConnectionDb connectionDb = new();
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
            if (!string.IsNullOrEmpty(IdCard))
            {
                ConnectionDb connectionDb = new();
                this.ExcListPatientAllergies(connectionDb, IdCard);
                List<AllergyViewModel> allergies = this.GetPatientAllergies(connectionDb);
                connectionDb.SqlConnection.Close();
                return Json(allergies);
            }
            return Json(null);
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
        public string UpdatePatientAllergy(string IdCard, string OldAllergyType, string AllergyType, string Description, string DiagnosticDate)
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
            connectionDb.CreateParameterOutput();
            connectionDb.ExcecuteReader();
            int resp = ReadValidateRegister(connectionDb);
            connectionDb.SqlConnection.Close();
            return resp.ToString();
        }

        [HttpGet]
        public JsonResult ListPatientMedicaments(string IdCard, string AllergyType)
        {
            ConnectionDb connectionDb = new ConnectionDb();
            this.ExcListPatientMedicaments(connectionDb, IdCard, AllergyType);
            List<string> medicaments = this.GetPatientMedicaments(connectionDb);
            connectionDb.SqlConnection.Close();
            return Json(medicaments);
        }

        public List<string> GetPatientMedicaments(ConnectionDb connectionDb)
        {
            List<string> medicaments = new List<string>();
            while (connectionDb.SqlDataReader.Read())
            {
                medicaments.Add(connectionDb.SqlDataReader["MEDICINE_TYPE"].ToString());
            }
            return medicaments;
        }

        private void ExcListPatientMedicaments(ConnectionDb connectionDb, string IdCard, string AllergyType)
        {
            string paramId = "@param_ID_CARD"
                , paramAllergyType = "@param_ALLERGY_TYPE"
                , commandText = "PATIENT.sp_LIST_PATIENT_MEDICAMENT";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramId, SqlDbType.VarChar, IdCard);
            connectionDb.CreateParameter(paramAllergyType, SqlDbType.VarChar, AllergyType);
            connectionDb.ExcecuteReader();
        }

        [HttpPost]
        public string AddPatientMedicament(string IdCard, string MedicineType, string AllergyType)
        {
            ConnectionDb connectionDb = new ConnectionDb();
            this.excAddMedicament(connectionDb, IdCard, MedicineType, AllergyType);
            connectionDb.SqlConnection.Close();
            return "1";
        }

        public void excAddMedicament(ConnectionDb connectionDb, string IdCard, string MedicineType, string AllergyType)
        {
            string paramId = "@param_ID_CARD"
                     , paramAllergyType = "@param_ALLERGY_TYPE"
                      , paramMedicineType = "@param_MEDICINE_TYPE"
                      , commandText = "PATIENT.sp_ADD_PACIENT_MEDICAMENT";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramId, SqlDbType.VarChar, IdCard);
            connectionDb.CreateParameter(paramAllergyType, SqlDbType.VarChar, AllergyType);
            connectionDb.CreateParameter(paramMedicineType, SqlDbType.VarChar, MedicineType);

            connectionDb.CreateParameterOutput();
            connectionDb.ExcecuteReader();
        }

        [HttpDelete]
        public string RemovePatientMedicament(string IdCard, string MedicineType, string AllergyType)
        {
            ConnectionDb connectionDb = new ConnectionDb();
            this.excRemoveMedicament(connectionDb, IdCard, MedicineType, AllergyType);
            connectionDb.SqlConnection.Close();
            return "1";
        }

        public void excRemoveMedicament(ConnectionDb connectionDb, string IdCard, string MedicineType, string AllergyType)
        {
            string paramId = "@param_ID_CARD"
                     , paramAllergyType = "@param_ALLERGY_TYPE"
                      , paramMedicineType = "@param_MEDICINE_TYPE"
                      , commandText = "PATIENT.REMOVE_PATIENT_MEDICAMENT";
            connectionDb.InitSqlComponents(commandText);
            connectionDb.CreateParameter(paramId, SqlDbType.VarChar, IdCard);
            connectionDb.CreateParameter(paramAllergyType, SqlDbType.VarChar, AllergyType);
            connectionDb.CreateParameter(paramMedicineType, SqlDbType.VarChar, MedicineType);

            connectionDb.CreateParameterOutput();
            connectionDb.ExcecuteReader();
        }

    }
}

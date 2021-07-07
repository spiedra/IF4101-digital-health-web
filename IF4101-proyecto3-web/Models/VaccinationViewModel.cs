using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IF4101_proyecto3_web.Models
{
    public class VaccinationViewModel
    {

        [Required]
        public int IdCard { get; set; }

        public string FullName { get; set; }

        [Required]
        public string VaccinationType { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ApplicationDate { get; set; }

        [Required]
        public string NextVaccinationDate { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IF4101_proyecto3_web.Models
{
    public class AllergyViewModel
    {
     
        [Required]
        public int IdCard { get; set; }

        public string FullName { get; set; }

        [Required]
        public string AllergyType { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string DiagnosticDate { get; set; }
    }
}

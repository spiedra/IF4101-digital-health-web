using System;
using System.ComponentModel.DataAnnotations;

namespace IF4101_proyecto3_web.Models
{
    public class AppointmentViewModel
    {
        [Required]
        public string PatientCard { get; set; }

        [Required]
        public string HealthCenter { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string SpecialityType { get; set; }

        [Required]
        public string Description { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace IF4101_proyecto3_web.Models
{
    public class DoctorViewModel
    {
        [Required]
        public string IdCard { get; set; }

        [Required]
        public string DoctorCode { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

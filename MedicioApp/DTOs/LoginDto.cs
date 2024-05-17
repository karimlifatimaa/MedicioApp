using System.ComponentModel.DataAnnotations;

namespace MedicioApp.DTOs
{
    public class LoginDto
    {
        [Required]
        public string EmailOrUsername { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsRemember { get; set; }
    }
}

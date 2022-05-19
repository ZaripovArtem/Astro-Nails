using System.ComponentModel.DataAnnotations;

namespace Astro_Nails.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

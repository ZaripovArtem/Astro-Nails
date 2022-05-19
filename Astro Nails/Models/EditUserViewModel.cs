using System.ComponentModel.DataAnnotations;

namespace Astro_Nails.Models
{
    public class EditUserViewModel
    {
        //[Required(ErrorMessage = "Не указан пароль")]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }
        //[DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "Пароль введен неверно")]
        //public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введите фамилию")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Введите отчество")]
        public string Patronomic { get; set; }
    }
}

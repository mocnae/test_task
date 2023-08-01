using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [Display(Name = "Введите новый пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string Password1 { get; set; }
    }
}
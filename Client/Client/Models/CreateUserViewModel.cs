using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class CreateUserViewModel
    {
        [Required]
        [Display(Name = "Введите имя пользователя:")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Введите Email:")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Выберите роль для пользователя:")]
        public string Role { get; set; }
        [Required]
        [Display(Name = "Введите пароль:")]
        public string Password { get; set; }
    }
}
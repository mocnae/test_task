using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password1 { get; set; }

        [Required]
        [Compare("Password1", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string Password2 { get; set; }
    }
}
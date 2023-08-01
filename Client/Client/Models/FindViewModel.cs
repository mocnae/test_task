using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class FindViewModel
    {
        [Display(Name = "Введите название продукта:")]
        public string Str { get; set; }
    }
}
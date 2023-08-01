using System.ComponentModel.DataAnnotations;
using Client.DTOs;

namespace Client.Models
{
    public class CategoryViewModel
    {
        [Required]
        public string Description { get; set; }

        public static explicit operator CategoryViewModel(CategoryDto v)
        {
            return new CategoryViewModel 
            {
                Description = v.Description
            };
        }
    }
}
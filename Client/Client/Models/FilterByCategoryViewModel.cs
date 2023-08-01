using Client.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class FilterByCategoryViewModel
    {
        [Display(Name = "Выберите категорию")]
        public int CategoryId { get; set; }
        public List<CategoryDto> Categories { get; set; }

        public static explicit operator FilterByCategoryViewModel(List<CategoryDto> v)
        {
            return new FilterByCategoryViewModel
            {
                Categories = v
            };
        }
    }
}
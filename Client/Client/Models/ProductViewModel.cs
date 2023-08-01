using Client.DTOs;

namespace Client.Models
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
        public string GeneralNote { get; set; }
        public string SpecialNote { get; set; }

        public static implicit operator ProductViewModel(ProductDto v)
        {
            return new ProductViewModel
            {
                Name = v.Name,
                CategoryId = v.CategoryId,
                Description = v.Description,
                Cost = v.Cost,
                GeneralNote = v.GeneralNote,
                SpecialNote = v.SpecialNote
            };
        }
    }
}
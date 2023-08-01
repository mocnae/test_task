using Client.Models;

namespace Client.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
        public string GeneralNote { get; set; }
        public string SpecialNote { get; set; }

        public static implicit operator ProductDto(ProductViewModel v)
        {
            return new ProductDto
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
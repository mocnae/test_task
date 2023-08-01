using Client.Models;

namespace Client.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public static explicit operator CategoryDto(CategoryViewModel v)
        {
            return new CategoryDto
            {
                Description = v.Description
            };
        }
    }
}
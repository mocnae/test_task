using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Category
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public uint Cost { get; set; }
        public string GeneralNote { get; set; }
        public string SpecialNote { get; set; }
    }
}
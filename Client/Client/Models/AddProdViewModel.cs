namespace Client.Models
{
    public class AddProdViewModel
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
        public string GeneralNote { get; set; }
        public string SpecialNote { get; set; }
    }
}
namespace ChushkaWebApp.Services.Models
{
    using ChushkaWebApp.Models.Enums;

    public class ProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public ProductType Type { get; set; }
    }
}

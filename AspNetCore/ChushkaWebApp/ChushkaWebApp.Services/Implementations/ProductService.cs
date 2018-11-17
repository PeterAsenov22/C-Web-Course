using ChushkaWebApp.Data;
using ChushkaWebApp.Models;
using ChushkaWebApp.Models.Enums;
using ChushkaWebApp.Services.Interfaces;

namespace ChushkaWebApp.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ChushkaDbContext db;

        public ProductService(ChushkaDbContext db)
        {
            this.db = db;
        }

        public void Create(string name, decimal price, string description, ProductType type)
        {
            var product = new Product()
            {
                Name = name,
                Description = description,
                Price = price,
                Type = type
            };

            var result = this.db.Products.AddAsync(product).Result;
            this.db.SaveChanges();
        }
    }
}

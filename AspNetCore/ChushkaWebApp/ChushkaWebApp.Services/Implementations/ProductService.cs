namespace ChushkaWebApp.Services.Implementations
{
    using ChushkaWebApp.Models;
    using ChushkaWebApp.Models.Enums;
    using Data;
    using Interfaces;
    using Models;
    using System.Linq;
    using System.Collections.Generic;

    public class ProductService : IProductService
    {
        private readonly ChushkaDbContext db;

        public ProductService(ChushkaDbContext db)
        {
            this.db = db;
        }

        public int Create(string name, decimal price, string description, ProductType type)
        {
            var product = new Product()
            {
                Name = name,
                Description = description,
                Price = price,
                Type = type
            };

            this.db.Products.Add(product);
            this.db.SaveChanges();

            return product.Id;
        }

        public void Edit(int id, string name, decimal price, string description, ProductType type)
        {
            var product = this.db.Products.First(p => p.Id == id);

            product.Name = name;
            product.Description = description;
            product.Price = price;
            product.Type = type;

            this.db.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = this.db.Products.First(p => p.Id == id);

            this.db.Products.Remove(product);
            this.db.SaveChanges();
        }

        public bool Exists(int id)
        {
            return this.db.Products.Any(p => p.Id == id);
        }

        public ProductModel FindById(int id)
        {
            return this.db
                .Products
                .Where(p => p.Id == id)
                .Select(p => new ProductModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    Type = p.Type
                })
                .First();
        }

        public IEnumerable<ProductModel> All()
        {
            return this.db
                .Products
                .Select(p => new ProductModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Type = p.Type
                })
                .ToList();
        }
    }
}

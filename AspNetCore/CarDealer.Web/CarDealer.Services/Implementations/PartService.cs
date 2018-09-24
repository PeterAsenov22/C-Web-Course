namespace CarDealer.Services.Implementations
{
    using Data;
    using Data.Models;
    using Models.Parts;
    using System.Collections.Generic;
    using System.Linq;

    public class PartService : IPartService
    {
        private readonly CarDealerDbContext db;

        public PartService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<PartListingModel> AllListings(int page = 1, int pageSize = 10)
        {
            return this.db
                .Parts
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PartListingModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SupplierName = p.Supplier.Name
                })
                .ToList();
        }

        public IEnumerable<PartBasicModel> All()
        {
            return this.db
                .Parts
                .OrderBy(p => p.Id)
                .Select(p => new PartBasicModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();
        }

        public PartEditModel ById(int id)
        {
            return this.db
                .Parts
                .Where(p => p.Id == id)
                .Select(p => new PartEditModel
                {
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity
                })
                .FirstOrDefault();
        }

        public int Count()
        {
            return this.db.Parts.Count();
        }

        public void Create(string name, decimal price, int quantity, int supplierId)
        {
            var part = new Part
            {
                Name = name,
                Price = price,
                Quantity = quantity,
                SupplierId = supplierId
            };

            this.db.Parts.Add(part);
            this.db.SaveChanges();
        }

        public void Edit(int id, decimal price, int quantity)
        {
            var part = this.db.Parts.Find(id);

            if (part is null)
            {
                return;
            }

            part.Price = price;
            part.Quantity = quantity;
            this.db.SaveChanges();
        }

        public void Delete(int id)
        {
            var part = this.db.Parts.Find(id);

            if (part is null)
            {
                return;
            }

            this.db.Parts.Remove(part);
            this.db.SaveChanges();
        }
    }
}

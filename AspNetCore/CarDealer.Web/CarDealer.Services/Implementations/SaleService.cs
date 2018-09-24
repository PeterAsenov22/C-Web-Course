using CarDealer.Data.Models;

namespace CarDealer.Services.Implementations
{
    using Data;
    using Models.Cars;
    using Models.Sales;
    using System.Collections.Generic;
    using System.Linq;

    public class SaleService : ISaleService
    {
        private readonly CarDealerDbContext db;

        public SaleService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<SaleListModel> All()
        {
            return this.db
                .Sales
                .OrderByDescending(s => s.Id)
                .Select(s => new SaleListModel
                {
                    Id = s.Id,
                    CustomerName = s.Customer.Name,
                    IsYoungDriver = s.Customer.IsYoungDriver,
                    Discount = s.Discount,
                    Price = s.Car.Parts.Sum(p => p.Part.Price)
                })
                .ToList();
        }

        public IEnumerable<SaleListModel> Discounted()
        {
            return this.db
                .Sales
                .Where(s => s.Discount > 0 || s.Customer.IsYoungDriver)
                .Select(s => new SaleListModel
                {
                    Id = s.Id,
                    CustomerName = s.Customer.Name,
                    IsYoungDriver = s.Customer.IsYoungDriver,
                    Discount = s.Discount,
                    Price = s.Car.Parts.Sum(p => p.Part.Price)
                })
                .ToList();
        }

        public SaleDetailsModel Details(int id)
        {
            return this.db
                .Sales
                .Where(s => s.Id == id)
                .Select(s => new SaleDetailsModel
                {
                    Id = s.Id,
                    CustomerName = s.Customer.Name,
                    IsYoungDriver = s.Customer.IsYoungDriver,
                    Discount = s.Discount,
                    Price = s.Car.Parts.Sum(p => p.Part.Price),
                    Car = new CarModel
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    }
                })
                .FirstOrDefault();
        }

        public void Create(int customerId, int carId, double discount)
        {
            var sale = new Sale
            {
                CustomerId = customerId,
                CarId = carId,
                Discount = discount
            };

            this.db.Sales.Add(sale);
            this.db.SaveChanges();
        }
    }
}

namespace CarDealer.Services.Implementations
{
    using Data;
    using Data.Models;
    using Models.Parts;
    using Models.Cars;
    using System.Collections.Generic;
    using System.Linq;

    public class CarService : ICarService
    {
        private readonly CarDealerDbContext db;

        public CarService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<CarModel> All()
        {
            return db
                .Cars
                .OrderBy(c => c.Make)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new CarModel
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .ToList();
        }

        public IEnumerable<CarIdNameModel> AllWithFullName()
        {
            return db
                .Cars
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Select(c => new CarIdNameModel
                {
                    Id = c.Id,
                    Name = $"{c.Make} {c.Model}"
                })
                .ToList();
        }

        public IEnumerable<CarModel> ByMake(string make)
        {
            return db
                .Cars
                .Where(c => c.Make.ToLower() == make.ToLower())
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new CarModel
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .ToList();
        }

        public CarWithPartsModel WithParts(int id)
        {
            return this.db
                .Cars
                .Where(c => c.Id == id)
                .Select(c => new CarWithPartsModel
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.Parts.Select(p => new PartModel
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    })
                })
                .FirstOrDefault();
        }

        public void Create(string make, string model, long travelledDistance, IEnumerable<int> parts)
        {
            var existingPartIds = this.db
                .Parts
                .Where(p => parts.Contains(p.Id))
                .Select(p => p.Id)
                .ToList();

            var car = new Car
            {
                Make = make,
                Model = model,
                TravelledDistance = travelledDistance,
            };

            foreach (var partId in existingPartIds)
            {
                car.Parts.Add(new PartCar
                {
                    PartId = partId
                });
            }

            this.db.Cars.Add(car);
            this.db.SaveChanges();
        }

        public CarByIdModel ById(int id)
        {
            return this.db
                .Cars
                .Where(c => c.Id == id)
                .Select(c => new CarByIdModel
                {
                    FullName = $"{c.Make} {c.Model}",
                    Price = c.Parts.Sum(p => p.Part.Price)
                })
                .FirstOrDefault();
        }

        public bool Exists(int id)
        {
            return this.db.Cars.Any(c => c.Id == id);
        }
    }
}

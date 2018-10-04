namespace CameraBazaar.Services.Implementations
{
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using Models.Camera;
    using System.Linq;
    using System.Collections.Generic;

    public class CameraService : ICameraService
    {
        private readonly CameraBazaarDbContext db;

        public CameraService(CameraBazaarDbContext db)
        {
            this.db = db;
        }

        public void Create(
            CameraMake make,
            string model,
            decimal price,
            int quantity,
            int minShutterSpeed,
            int maxShutterSpeed,
            MinISO minISO,
            int maxISO,
            bool isFullName,
            string videoResolution,
            IEnumerable<LightMetering> lightMeterings,
            string description,
            string imageUrl,
            string userId)
        {
            var camera = new Camera
            {
                Make = make,
                Model = model,
                Price = price,
                Quantity = quantity,
                MinShutterSpeed = minShutterSpeed,
                MaxShutterSpeed = maxShutterSpeed,
                MinISO = minISO,
                MaxISO = maxISO,
                IsFullFrame = isFullName,
                VideoResolution = videoResolution,
                LightMetering = (LightMetering)lightMeterings.Cast<int>().Sum(),
                Description = description,
                ImageUrl = imageUrl,
                UserId = userId
            };

            this.db.Cameras.Add(camera);
            this.db.SaveChanges();
        }

        public IEnumerable<CameraModel> GetUserCameras(string userId)
        {
            return this.db
                .Cameras
                .Where(c => c.UserId == userId)
                .Select(c => new CameraModel
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    Price = c.Price,
                    Quantity = c.Quantity,
                    ImageUrl = c.ImageUrl
                });
        }

        public IEnumerable<CameraModel> All()
        {
            return this.db
                .Cameras
                .Select(c => new CameraModel
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    ImageUrl = c.ImageUrl,
                    Price = c.Price,
                    Quantity = c.Quantity
                });
        }

        public CameraFullModel DetailsById(int id)
        {
            return this.db
                .Cameras
                .Where(c => c.Id == id)
                .Select(c => new CameraFullModel
                {
                    Description = c.Description,
                    Id = c.Id,
                    ImageUrl = c.ImageUrl,
                    IsFullFrame = c.IsFullFrame,
                    LightMetering = c.LightMetering,
                    Make = c.Make,
                    MaxISO = c.MaxISO,
                    MaxShutterSpeed = c.MaxShutterSpeed,
                    MinISO = c.MinISO,
                    MinShutterSpeed = c.MinShutterSpeed,
                    Model = c.Model,
                    Quantity = c.Quantity,
                    Price = c.Price,
                    VideoResolution = c.VideoResolution,
                    Seller = c.User.UserName
                })
                .FirstOrDefault();
        }
    }
}

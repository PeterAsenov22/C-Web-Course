namespace CameraBazaar.Services
{
    using Data.Models.Enums;
    using Models.Camera;
    using System.Collections.Generic;

    public interface ICameraService
    {
        void Create(
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
            string userId);

        IEnumerable<CameraModel> GetUserCameras(string userId);

        IEnumerable<CameraModel> All();

        CameraFullModel DetailsById(int id);
    }
}

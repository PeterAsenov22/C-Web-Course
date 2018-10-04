namespace CameraBazaar.Services.Models.Camera
{
    using Data.Models.Enums;

    public class CameraModel
    {
        public int Id { get; set; }

        public  CameraMake Make { get; set; }

        public string Model { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string ImageUrl { get; set; }
    }
}

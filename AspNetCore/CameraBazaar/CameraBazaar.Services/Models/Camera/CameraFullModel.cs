namespace CameraBazaar.Services.Models.Camera
{
    using Data.Models.Enums;

    public class CameraFullModel : CameraModel
    {
        public int MinShutterSpeed { get; set; }

        public int MaxShutterSpeed { get; set; }

        public MinISO MinISO { get; set; }

        public int MaxISO { get; set; }

        public bool IsFullFrame { get; set; }

        public string VideoResolution { get; set; }

        public LightMetering LightMetering { get; set; }

        public string Description { get; set; }

        public string Seller { get; set; }
    }
}

namespace CameraBazaar.Web.Models.CamerasViewModels
{
    using Data.Models.Enums;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AddCameraViewModel
    {
        public CameraMake Make { get; set; }

        [Required]
        [MaxLength(100)]
        public string Model { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, 100)]
        public int Quantity { get; set; }

        [Range(1, 30)]
        [Display(Name = "Min Shutter Speed")]
        public int MinShutterSpeed { get; set; }

        [Range(2000, 8000)]
        [Display(Name = "Max Shutter Speed")]
        public int MaxShutterSpeed { get; set; }

        [Display(Name = "Min ISO")]
        public MinISO MinISO { get; set; }

        [Range(200, 409600)]
        [Display(Name = "Max ISO")]
        public int MaxISO { get; set; }

        [Display(Name = "Full Frame")]
        public bool IsFullFrame { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Video Resolution")]
        public string VideoResolution { get; set; }

        [Display(Name = "Light Metering")]
        public IEnumerable<LightMetering> LightMeterings { get; set; }

        [Required]
        [MaxLength(6000)]
        public string Description { get; set; }

        [Required]
        [MinLength(10)]
        [StringLength(2000, MinimumLength = 10)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }
    }
}

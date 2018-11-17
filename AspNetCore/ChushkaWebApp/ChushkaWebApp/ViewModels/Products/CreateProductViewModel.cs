namespace ChushkaWebApp.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    public class CreateProductViewModel
    {
        [Required]
        [MinLength(4, ErrorMessage = "Product name must be at least 4 characters long")]
        [MaxLength(20, ErrorMessage = "Product name must be not more than 20 characters long")]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 60 characters long")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public string Type { get; set; }
    }
}

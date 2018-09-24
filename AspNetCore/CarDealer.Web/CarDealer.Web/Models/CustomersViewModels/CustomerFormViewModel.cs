namespace CarDealer.Web.Models.CustomersViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CustomerFormViewModel
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Display(Name = "Birthday")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Young Driver")]
        public bool IsYoungDriver { get; set; }
    }
}

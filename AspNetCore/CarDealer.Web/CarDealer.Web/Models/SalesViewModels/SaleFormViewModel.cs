namespace CarDealer.Web.Models.SalesViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class SaleFormViewModel
    {
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Display(Name = "Car")]
        public int CarId { get; set; }

        [Range(0, 100)]
        public double Discount { get; set; }
   
        public IEnumerable<SelectListItem> Customers { get; set; }
        
        public IEnumerable<SelectListItem> Cars { get; set; }
    }
}

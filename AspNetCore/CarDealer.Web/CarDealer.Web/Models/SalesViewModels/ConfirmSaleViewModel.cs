namespace CarDealer.Web.Models.SalesViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class ConfirmSaleViewModel
    {
        public int CustomerId { get; set; }

        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        public bool IsYoungDriver { get; set; }

        public int CarId { get; set; }

        [Display(Name = "Car")]
        public string CarFullName { get; set; }

        public double Discount { get; set; }

        public double FinalDiscount => IsYoungDriver ? Discount + 5 : Discount;

        public decimal CarPrice { get; set; }

        public decimal FinalPrice => CarPrice * (1 - ((decimal) FinalDiscount / 100));
    }
}

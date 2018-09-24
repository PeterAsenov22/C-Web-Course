namespace CarDealer.Services.Models.Customers
{
    using System;

    public class CustomerModel : CustomerIdNameModel
    {
        public DateTime BirthDate { get; set; }

        public bool IsYoungDriver { get; set; }
    }
}

namespace CarDealer.Web.Models.CustomersViewModels
{
    using System.Collections.Generic;
    using Services.Models;
    using Services.Models.Customers;

    public class AllCustomersViewModel
    {
        public IEnumerable<CustomerModel> Customers { get; set; }

        public OrderDirection OrderDirection { get; set; }
    }
}

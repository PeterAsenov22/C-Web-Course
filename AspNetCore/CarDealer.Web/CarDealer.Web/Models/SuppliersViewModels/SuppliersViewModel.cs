namespace CarDealer.Web.Models.SuppliersViewModels
{
    using Services.Models.Suppliers;
    using System.Collections.Generic;

    public class SuppliersViewModel
    {
        public string Type { get; set; }

        public IEnumerable<SupplierListingModel> Suppliers { get; set; }
    }
}

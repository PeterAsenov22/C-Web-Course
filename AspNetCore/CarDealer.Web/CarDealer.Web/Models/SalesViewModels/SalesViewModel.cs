namespace CarDealer.Web.Models.SalesViewModels
{
    using Services.Models.Sales;
    using System.Collections.Generic;

    public class SalesViewModel
    {
        public string Type { get; set; }

        public IEnumerable<SaleListModel> Sales { get; set; }
    }
}

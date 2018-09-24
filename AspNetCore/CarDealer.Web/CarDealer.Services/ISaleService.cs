namespace CarDealer.Services
{
    using Models.Sales;
    using System.Collections.Generic;

    public interface ISaleService
    {
        IEnumerable<SaleListModel> All();

        IEnumerable<SaleListModel> Discounted();
        
        SaleDetailsModel Details(int id);

        void Create(int customerId, int carId, double discount);
    }
}

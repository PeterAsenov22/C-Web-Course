namespace ChushkaWebApp.Services.Interfaces
{
    using System.Collections.Generic;
    using Models;

    public interface IOrderService
    {
        void Create(string userId, int productId);

        IEnumerable<OrderModel> All();
    }
}

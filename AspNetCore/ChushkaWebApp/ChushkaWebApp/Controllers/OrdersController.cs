namespace ChushkaWebApp.Web.Controllers
{
    using System.Linq;
    using Services.Interfaces;
    using ViewModels.Orders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : Controller
    {
        private readonly IOrderService orders;

        public OrdersController(IOrderService orders)
        {
            this.orders = orders;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult All()
        {
            var allOrdersViewModel = new AllOrdersViewModel
            {
                Orders = this.orders.All().Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    Customer = o.Customer,
                    Product = o.Product,
                    OrderedOn = o.OrderedOn
                })
            };

            return this.View(allOrdersViewModel);
        }
    }
}

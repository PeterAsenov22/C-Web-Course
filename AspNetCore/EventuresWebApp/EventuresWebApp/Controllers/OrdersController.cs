namespace EventuresWebApp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services.Interfaces;
    using System.Threading.Tasks;
    using System.Linq;
    using ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly IOrderService orders;
        private readonly IEventService events;
        private readonly UserManager<EventuresUser> userManager;

        public OrdersController(IOrderService orders, IEventService events, UserManager<EventuresUser> userManager)
        {
            this.orders = orders;
            this.events = events;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Order(string eventId, int ticketsCount)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            if (ticketsCount > 0 && events.Exists(eventId))
            {
                this.orders.Order(eventId, user.Id, ticketsCount);
            }

            return this.RedirectToAction("My", "Events");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult All()
        {
            var allOrdersViewModel = new AllOrdersViewModel()
            {
                Orders = this.orders.All().Select(o => new OrderViewModel()
                {
                    Customer = o.Customer,
                    EventName = o.EventName,
                    OrderedOn = o.OrderedOn
                })
            };

            return View(allOrdersViewModel);
        }
    }
}
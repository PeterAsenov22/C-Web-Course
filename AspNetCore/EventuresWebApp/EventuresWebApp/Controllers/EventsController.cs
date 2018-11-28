namespace EventuresWebApp.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Services.Interfaces;
    using ViewModels.Events;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Filters;
    using Models;
    using Microsoft.AspNetCore.Identity;

    public class EventsController : Controller
    {
        private readonly IEventService events;
        private readonly IOrderService orders;
        private readonly UserManager<EventuresUser> userManager;
        private readonly ILogger logger;

        public EventsController(
            IEventService events, 
            IOrderService orders,
            UserManager<EventuresUser> userManager,
            ILogger<EventsController> logger)
        {
            this.events = events;
            this.orders = orders;
            this.userManager = userManager;
            this.logger = logger;
        }

        [Authorize]
        public IActionResult All()
        {
            var eventsViewModel = new AllEventsViewModel()
            {
                Events = this.events.All().Select(e => new EventViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Start = e.Start,
                    End = e.End
                })
            };

            return View(eventsViewModel);
        }

        [Authorize]
        public async Task<IActionResult> My()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            var eventsViewModel = new AllEventsViewModel()
            {
                Events = this.orders.ByUserId(user.Id).Select(e => new EventViewModel
                {
                    Name = e.EventName,
                    Start = e.EventStart,
                    End = e.EventEnd,
                    TicketsCount = e.TicketsCount
                })
            };

            return View(eventsViewModel);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [ServiceFilter(typeof(LogAdminActivityActionFilter))]
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Create(CreateEventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.events.Create(
                model.Name,
                model.Place,
                model.Start,
                model.End,
                model.TotalTickets,
                model.PricePerTicket);

            this.logger.LogInformation("Event created: " + model.Name);

            return this.RedirectToAction("All");
        }
    }
}

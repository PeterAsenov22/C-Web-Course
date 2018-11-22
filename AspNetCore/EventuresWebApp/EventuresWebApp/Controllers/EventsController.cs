namespace EventuresWebApp.Web.Controllers
{
    using System.Linq;
    using Services.Interfaces;
    using ViewModels.Events;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Filters;

    public class EventsController : Controller
    {
        private readonly IEventService events;
        private readonly ILogger logger;

        public EventsController(IEventService events, ILogger<EventsController> logger)
        {
            this.events = events;
            this.logger = logger;
        }

        [Authorize]
        public IActionResult All()
        {
            var eventsViewModel = new AllEventsViewModel()
            {
                Events = this.events.All().Select(e => new EventViewModel()
                {
                    Name = e.Name,
                    Place = e.Place,
                    Start = e.Start,
                    End = e.End
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

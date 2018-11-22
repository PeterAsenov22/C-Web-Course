namespace EventuresWebApp.Services.Implementations
{
    using System;
    using EventuresWebApp.Models;
    using System.Linq;
    using Data;
    using System.Collections.Generic;
    using Interfaces;
    using Models;

    public class EventService : IEventService
    {
        private readonly EventuresDbContext db;

        public EventService(EventuresDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<EventModel> All()
        {
            return this.db
                .Events
                .Select(e => new EventModel
                {
                    Name = e.Name,
                    Start = string.Format("{0:g}", e.Start),
                    End = string.Format("{0:g}", e.End),
                    Place = e.Place
                })
                .ToList();
        }

        public void Create(string name, string place, DateTime start, DateTime end, int totalTickets, decimal pricePerTicket)
        {
            Event _event = new Event
            {
                Name = name,
                Place = place,
                Start = start,
                End = end,
                TotalTickets = totalTickets,
                PricePerTicket = pricePerTicket
            };

            this.db.Events.Add(_event);
            this.db.SaveChanges();
        }

        public EventModel Last()
        {
            return this.db
                .Events
                .Select(e => new EventModel()
                {
                    Name = e.Name,
                    Start = string.Format("{0:g}", e.Start),
                    End = string.Format("{0:g}", e.End),
                    Place = e.Place
                })
                .Last();
        }
    }
}

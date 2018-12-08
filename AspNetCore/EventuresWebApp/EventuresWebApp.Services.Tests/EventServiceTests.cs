namespace EventuresWebApp.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using EventuresWebApp.Models;
    using Implementations;
    using Models;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class EventServiceTests
    {
        private readonly EventuresDbContext dbContext;
        private readonly EventService events;
        private DbContextOptions<EventuresDbContext> options;

        public EventServiceTests()
        {
            this.options = new DbContextOptionsBuilder<EventuresDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this.dbContext = new EventuresDbContext(options);
            this.events = new EventService(dbContext);
        }

        [Fact]
        public void CreateShouldCreateEventSuccessfully()
        {
            events.Create("Event1", "Sofia", DateTime.MinValue, DateTime.MinValue, 100, 10);
            var _event = dbContext.Events.First();

            Assert.Equal("Event1", _event.Name);
            Assert.Equal("Sofia", _event.Place);
            Assert.Equal(DateTime.MinValue, _event.Start);
            Assert.Equal(DateTime.MinValue, _event.End);
            Assert.Equal(100, _event.TotalTickets);
            Assert.Equal(100, _event.TicketsLeft);
            Assert.Equal(10, _event.PricePerTicket);
        }

        [Fact]
        public void AllShouldReturnValidCollectionOfEventModels()
        {
            var eventsList = new List<Event>()
            {
                new Event() { Id = "1", Name = "Event1", TicketsLeft = 4 },
                new Event() { Id = "2", Name = "Event2", TicketsLeft = 4 },
                new Event() { Id = "3", Name = "Event3", TicketsLeft = 4 },
                new Event() { Id = "4", Name = "Event4", TicketsLeft = 4 }
            };

            dbContext.Events.AddRange(eventsList);
            dbContext.SaveChanges();

            var eventsModels = new List<EventModel>()
            {
                new EventModel { Name = "Event1" },
                new EventModel { Name = "Event2" },
                new EventModel { Name = "Event3" },
                new EventModel { Name = "Event4" }
            };

            Assert.Equal(
              events.All(1, 4).Select(e => e.Name).ToList(),
              eventsModels.Select(e => e.Name).ToList());
        }

        [Fact]
        public void LastShouldReturnValidEventModel()
        {
            var eventsList = new List<Event>()
            {
                new Event() { Id = "1", Name = "Event1", TicketsLeft = 4 },
                new Event() { Id = "2", Name = "Event2", TicketsLeft = 4 },
                new Event() { Id = "3", Name = "Event3", TicketsLeft = 4 },
                new Event() { Id = "4", Name = "Event4", TicketsLeft = 4 }
            };

            dbContext.Events.AddRange(eventsList);
            dbContext.SaveChanges();

            var eventModel = new EventModel
            {
                Id = "1",
                Name = "Event4"
            };

            Assert.Equal(eventModel.Name, events.Last().Name);
        }

        [Fact]
        public void CountShouldReturnValidNumberOfAvailableEvents()
        {
            dbContext.Events.Add(new Event() { TicketsLeft = 4 });
            dbContext.Events.Add(new Event() { TicketsLeft = 4 });
            dbContext.Events.Add(new Event() { TicketsLeft = 4 });
            dbContext.Events.Add(new Event() { TicketsLeft = 4 });
            dbContext.Events.Add(new Event() { TicketsLeft = 4 });
            dbContext.Events.Add(new Event() { TicketsLeft = 0 });
            dbContext.SaveChanges();

            Assert.Equal(5, events.Count());
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        [InlineData("4")]
        public void ExistsShouldReturnTrue(string id)
        {
            var eventsList = new List<Event>()
            {
                new Event() { Id = "1", Name = "Event1", TicketsLeft = 4 },
                new Event() { Id = "2", Name = "Event2", TicketsLeft = 4 },
                new Event() { Id = "3", Name = "Event3", TicketsLeft = 4 },
                new Event() { Id = "4", Name = "Event4", TicketsLeft = 4 }
            };

            dbContext.Events.AddRange(eventsList);
            dbContext.SaveChanges();

            Assert.True(events.Exists(id));
        }

        [Fact]
        public void ExistsShouldReturnFalse()
        {
            var eventsList = new List<Event>()
            {
                new Event() { Id = "1", Name = "Event1", TicketsLeft = 4 },
                new Event() { Id = "2", Name = "Event2", TicketsLeft = 4 },
                new Event() { Id = "3", Name = "Event3", TicketsLeft = 4 },
                new Event() { Id = "4", Name = "Event4", TicketsLeft = 4 }
            };

            dbContext.Events.AddRange(eventsList);
            dbContext.SaveChanges();

            Assert.False(events.Exists("5"));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void TicketsLeftByIdShouldReturnCorrectValues(int id)
        {
            var eventsList = new List<Event>()
            {
                new Event() { Id = "1", Name = "Event1", TicketsLeft = 4 },
                new Event() { Id = "2", Name = "Event2", TicketsLeft = 5 },
                new Event() { Id = "3", Name = "Event3", TicketsLeft = 6 },
                new Event() { Id = "4", Name = "Event4", TicketsLeft = 7 }
            };

            dbContext.Events.AddRange(eventsList);
            dbContext.SaveChanges();

            int expectedTicketsLeft = eventsList[id-1].TicketsLeft;

            Assert.Equal(expectedTicketsLeft, events.TicketsLeftById(id.ToString()));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void ReduceTicketsLeftCountShouldReduceTicketsLeftCountCorrectly(int id)
        {
            var eventsList = new List<Event>()
            {
                new Event() { Id = "1", Name = "Event1", TicketsLeft = 4 },
                new Event() { Id = "2", Name = "Event2", TicketsLeft = 5 },
                new Event() { Id = "3", Name = "Event3", TicketsLeft = 6 },
                new Event() { Id = "4", Name = "Event4", TicketsLeft = 7 }
            };

            dbContext.Events.AddRange(eventsList);
            dbContext.SaveChanges();

            events.ReduceTicketsLeftCount(id.ToString(), eventsList[id - 1].TicketsLeft - 1);

            Assert.Equal(1, events.TicketsLeftById(id.ToString()));
        }
    }
}

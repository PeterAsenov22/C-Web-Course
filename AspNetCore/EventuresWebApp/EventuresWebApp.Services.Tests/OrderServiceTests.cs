namespace EventuresWebApp.Services.Tests
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using EventuresWebApp.Models;
    using Data;
    using Implementations;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class OrderServiceTests
    {
        private readonly IServiceProvider provider;
        private readonly EventuresDbContext context;        
        private readonly IOrderService orderService;    

        public OrderServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<EventuresDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IOrderService, OrderService>();

            this.provider = services.BuildServiceProvider();
            this.context = provider.GetService<EventuresDbContext>();
            this.orderService = provider.GetService<IOrderService>();
        }

        [Fact]
        public void OrderShouldOrderTicketsSuccessfully()
        {
            this.orderService.Order("event1", "customer1", 100);

            var order = context.Orders.First();

            Assert.Equal("customer1", order.CustomerId);
            Assert.Equal("event1", order.EventId);
            Assert.Equal(100, order.TicketsCount);
        }

        [Theory]
        [InlineData("customer1")]
        public void ByUserIdShouldReturnCorrectValues(string id)
        {
            var events = new List<Event>()
            {
                new Event()
                {
                    Id = "event1",
                    Name = "Event1",
                    Start = DateTime.MinValue,
                    End = DateTime.MinValue,
                    Place = "Sofia",
                    TotalTickets = 100,
                    TicketsLeft = 100,
                    PricePerTicket = 10
                },
                new Event()
                {
                    Id = "event2",
                    Name = "Event2",
                    Start = DateTime.MinValue,
                    End = DateTime.MinValue,
                    Place = "Sofia",
                    TotalTickets = 100,
                    TicketsLeft = 100,
                    PricePerTicket = 10
                }
            };

            var orders = new List<Order>()
            {
                new Order()
                {
                    Id = "order1",
                    CustomerId = id,
                    EventId = "event1",
                    TicketsCount = 10,
                    OrderedOn = DateTime.MinValue
                },
                new Order()
                {
                    Id = "order2",
                    CustomerId = id,
                    EventId = "event2",
                    TicketsCount = 100,
                    OrderedOn = DateTime.MinValue
                }
            };

            context.Events.AddRange(events);
            context.Orders.AddRange(orders);
            context.SaveChanges();

            var userOrders = orderService.ByUserId(id);

            Assert.Equal(2, userOrders.Count());
            Assert.Equal("Event1", userOrders.First().EventName);
            Assert.Equal("Event2", userOrders.Last().EventName);
            Assert.Equal(10, userOrders.First().TicketsCount);
            Assert.Equal(100, userOrders.Last().TicketsCount);
            Assert.Equal(string.Format("{0:g}", DateTime.MinValue), userOrders.First().EventStart);
            Assert.Equal(string.Format("{0:g}", DateTime.MinValue), userOrders.First().EventEnd);
            Assert.Equal(string.Format("{0:g}", DateTime.MinValue), userOrders.Last().EventStart);
            Assert.Equal(string.Format("{0:g}", DateTime.MinValue), userOrders.Last().EventEnd);
        }
    }
}

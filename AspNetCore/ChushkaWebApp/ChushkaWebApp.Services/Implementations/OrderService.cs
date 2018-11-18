namespace ChushkaWebApp.Services.Implementations
{
    using ChushkaWebApp.Models;
    using Data;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public class OrderService : IOrderService
    {
        private readonly ChushkaDbContext db;

        public OrderService(ChushkaDbContext db)
        {
            this.db = db;
        }

        public void Create(string userId, int productId)
        {
            var order = new Order
            {
                ClientId = userId,
                ProductId = productId,
                OrderedOn = DateTime.Now
            };

            this.db.Orders.Add(order);
            this.db.SaveChanges();
        }

        public IEnumerable<OrderModel> All()
        {
            return this.db
                .Orders
                .Select(o => new OrderModel
                {
                    Id = o.Id,
                    Customer = o.Client.FullName,
                    Product = o.Product.Name,
                    OrderedOn = o.OrderedOn.ToString()
                })
                .ToList();
        }
    }
}

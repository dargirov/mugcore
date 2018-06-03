using Microsoft.EntityFrameworkCore;
using MugStore.Data;
using MugStore.Data.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MugStore.Services.Data
{
    public class OrdersService : IOrdersService
    {
        private readonly IDbRepository<Order> orders;

        public OrdersService(IDbRepository<Order> orders)
        {
            this.orders = orders;
        }

        public void Create(Order order)
        {
            this.orders.Add(order);
            this.orders.Save();
        }

        public Order Get(int id)
        {
            return Get(x => x.Id == id);
        }

        public Order Get(string acronym)
        {
            return Get(x => x.Acronym == acronym);
        }

        public IQueryable<Order> Get()
        {
            return this.orders.All()
                .Include(x => x.DeliveryInfo)
                .ThenInclude(x => x.Courier)
                .Include(x => x.DeliveryInfo)
                .ThenInclude(x => x.City);
        }

        public void Save()
        {
            this.orders.Save();
        }

        private Order Get(Expression<Func<Order, bool>> predicate)
        {
            return this.orders.All()
                .Include(x => x.Product)
                .Include(x => x.Images)
                .Include(x => x.DeliveryInfo)
                .ThenInclude(x => x.Courier)
                .Include(x => x.DeliveryInfo)
                .ThenInclude(x => x.City)
                .FirstOrDefault(predicate);
        }
    }
}

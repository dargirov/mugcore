using MugStore.Data;
using MugStore.Data.Models;
using System.Linq;

namespace MugStore.Services.Data
{
    public class CouriersService : ICouriersService
    {
        private readonly IDbRepository<Courier> couriers;

        public CouriersService(IDbRepository<Courier> couriers)
        {
            this.couriers = couriers;
        }

        public IQueryable<Courier> Get()
        {
            return this.couriers.All();
        }

        public Courier Get(int id)
        {
            return this.couriers.GetById(id);
        }
    }
}

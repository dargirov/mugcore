using MugStore.Data;
using MugStore.Data.Models;
using System.Linq;

namespace MugStore.Services.Data
{
    public class BulletinsService : IBulletinsService
    {
        private readonly IDbRepository<Bulletin> bulletin;

        public BulletinsService(IDbRepository<Bulletin> bulletin)
        {
            this.bulletin = bulletin;
        }

        public void Add(Bulletin bulletin)
        {
            this.bulletin.Add(bulletin);
            this.bulletin.Save();
        }

        public Bulletin Get(string email)
        {
            return this.bulletin.All().Where(b => b.Email == email).FirstOrDefault();
        }

        public IQueryable<Bulletin> Get()
        {
            return this.bulletin.All();
        }
    }
}

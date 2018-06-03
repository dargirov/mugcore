using System.Linq;
using MugStore.Data.Models;

namespace MugStore.Services.Data
{
    public interface IBulletinsService
    {
        IQueryable<Bulletin> Get();

        Bulletin Get(string email);

        void Add(Bulletin bulletin);
    }
}

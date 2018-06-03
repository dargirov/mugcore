using System.Linq;
using MugStore.Data.Models;

namespace MugStore.Services.Data
{
    public interface ICitiesService
    {
        IQueryable<City> Get();

        City Get(int id);

        void Create(City city);

        void Save();
    }
}

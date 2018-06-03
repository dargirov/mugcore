using MugStore.Data;
using MugStore.Data.Models;
using System.Linq;

namespace MugStore.Services.Data
{
    public class CitiesService : ICitiesService
    {
        private readonly IDbRepository<City> cities;

        public CitiesService(IDbRepository<City> cities)
        {
            this.cities = cities;
        }

        public IQueryable<City> Get()
        {
            return this.cities.All().OrderByDescending(c => c.Highlight).ThenBy(c => c.PostCode);
        }

        public City Get(int id)
        {
            return this.cities.GetById(id);
        }

        public void Create(City city)
        {
            this.cities.Add(city);
            this.cities.Save();
        }

        public void Save()
        {
            this.cities.Save();
        }
    }
}

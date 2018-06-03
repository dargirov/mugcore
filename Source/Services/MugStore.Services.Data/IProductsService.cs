using MugStore.Data.Models;
using System.Linq;

namespace MugStore.Services.Data
{
    public interface IProductsService
    {
        void Create(Product product);

        Product Get(string acronym);

        Product Get(int id);

        IQueryable<Product> Get();

        void Save();

        IQueryable<Product> GetByTag(ProductTag tag);
    }
}

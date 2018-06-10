using Microsoft.EntityFrameworkCore;
using MugStore.Data;
using MugStore.Data.Models;
using System.Linq;

namespace MugStore.Services.Data
{
    public class ProductsService : IProductsService
    {
        private readonly IDbRepository<Product> products;

        public ProductsService(IDbRepository<Product> products)
        {
            this.products = products;
        }

        public void Create(Product product)
        {
            this.products.Add(product);
            this.products.Save();
        }

        public Product Get(string acronym)
        {
            return this.products.All()
                .Include(x => x.Images)
                .Include(x => x.Tags)
                .ThenInclude(x => x.ProductTag)
                .FirstOrDefault(p => p.Acronym == acronym);
        }

        public IQueryable<Product> Get()
        {
            return this.products.All();
        }

        public Product Get(int id)
        {
            return this.products.All()
                .Include(x => x.Images)
                .Include(x => x.Tags)
                .ThenInclude(x => x.ProductTag)
                .FirstOrDefault(x => x.Id == id);
        }

        public void Save()
        {
            this.products.Save();
        }
    }
}

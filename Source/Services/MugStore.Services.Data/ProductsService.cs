using Microsoft.EntityFrameworkCore;
using MugStore.Data;
using MugStore.Data.Models;
using System;
using System.Linq;

namespace MugStore.Services.Data
{
    public class ProductsService : IProductsService
    {
        private readonly IDbRepository<Product> products;
        private readonly IDbRepository<ProductTagProduct> productTagProducts;

        public ProductsService(IDbRepository<Product> products, IDbRepository<ProductTagProduct> productTagProducts)
        {
            this.products = products;
            this.productTagProducts = productTagProducts;
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
                .Include(x => x.ProductTags)
                .ThenInclude(x => x.ProductTag)
                .Where(p => p.Acronym == acronym)
                .FirstOrDefault();
        }

        public IQueryable<Product> Get()
        {
            return this.products.All();
        }

        public Product Get(int id)
        {
            return this.products.All()
                .Where(x => x.Id == id)
                .Include(x => x.Images)
                .Include(x => x.ProductTags)
                .ThenInclude(x => x.ProductTag)
                .FirstOrDefault();
        }

        public void Save()
        {
            this.products.Save();
        }

        public IQueryable<Product> GetByTag(ProductTag tag)
        {
            return this.productTagProducts.All()
                .Where(x => x.ProductTag == tag)
                .Select(x => x.Product);
        }
    }
}

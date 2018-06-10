using Microsoft.EntityFrameworkCore;
using MugStore.Data;
using MugStore.Data.Models;
using System.Linq;

namespace MugStore.Services.Data
{
    public class TagsService : ITagsService
    {
        private readonly IDbRepository<ProductTag> productTags;
        private readonly IDbRepository<PostTag> postTags;

        public TagsService(IDbRepository<ProductTag> productTags, IDbRepository<PostTag> postTags)
        {
            this.productTags = productTags;
            this.postTags = postTags;
        }

        public void Create(ProductTag tag)
        {
            this.productTags.Add(tag);
            this.productTags.Save();
        }

        public void Create(PostTag tag)
        {
            this.postTags.Add(tag);
            this.postTags.Save();
        }

        public IQueryable<ProductTag> GetProductTag()
        {
            return this.productTags.All();
        }

        public IQueryable<PostTag> GetPostTag()
        {
            return this.postTags.All();
        }

        public ProductTag GetProductTag(int id)
        {
            return this.productTags.GetById(id);
        }

        public PostTag GetPostTag(int id)
        {
            return this.postTags.GetById(id);
        }

        public ProductTag GetProductTag(string acronym)
        {
            return this.productTags.All()
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Images)
                .FirstOrDefault(t => t.Acronym == acronym);
        }

        public PostTag GetPostTag(string acronym)
        {
            return this.postTags.All()
                .Include(x => x.Posts)
                .ThenInclude(x => x.Post)
                .FirstOrDefault(t => t.Acronym == acronym);
        }
    }
}

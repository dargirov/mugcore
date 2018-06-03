using Microsoft.EntityFrameworkCore;
using MugStore.Data;
using MugStore.Data.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MugStore.Services.Data
{
    public class BlogService : IBlogService
    {
        private readonly IDbRepository<Post> posts;

        public BlogService(IDbRepository<Post> posts)
        {
            this.posts = posts;
        }

        public IQueryable<Post> GetPosts(Expression<Func<Post, bool>> predicate)
        {
            return this.posts.All()
                .Where(predicate)
                .Include(x => x.Tags)
                .ThenInclude(x => x.PostTag)
                .OrderByDescending(x => x.CreatedOn);
        }

        public Post GetPost(string acronym)
        {
            return this.posts.All()
                .Include(x => x.Tags)
                .ThenInclude(x => x.PostTag)
                .FirstOrDefault(x => x.Acronym == acronym);
        }

        public Post GetPost(int id)
        {
            return this.posts.All()
                .Include(x => x.Tags)
                .ThenInclude(x => x.PostTag)
                .FirstOrDefault(x => x.Id == id);
        }

        public void Create(Post post)
        {
            this.posts.Add(post);
            this.posts.Save();
        }

        public void Save()
        {
            this.posts.Save();
        }
    }
}

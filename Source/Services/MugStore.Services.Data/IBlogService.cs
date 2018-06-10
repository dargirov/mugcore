using MugStore.Data.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MugStore.Services.Data
{
    public interface IBlogService
    {
        IQueryable<Post> GetPosts(Expression<Func<Post, bool>> predicate);

        Post GetPost(string acronym);

        Post GetPost(int id);

        void Create(Post post);

        void Save();
    }
}

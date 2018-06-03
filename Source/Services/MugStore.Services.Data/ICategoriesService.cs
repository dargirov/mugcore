using System.Linq;
using MugStore.Data.Models;

namespace MugStore.Services.Data
{
    public interface ICategoriesService
    {
        IQueryable<Category> Get();

        Category Get(string acronym);

        void Create(Category category);
    }
}

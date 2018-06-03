using System.Linq;
using MugStore.Data.Models;

namespace MugStore.Services.Data
{
    public interface IFeedbacksService
    {
        IQueryable<Feedback> Get();

        void Add(Feedback feedback);

        void Save();

        void Delete(Feedback feedback);
    }
}

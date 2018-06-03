using System.Linq;
using MugStore.Data.Models;

namespace MugStore.Services.Data
{
    public interface ICouriersService
    {
        IQueryable<Courier> Get();

        Courier Get(int id);
    }
}

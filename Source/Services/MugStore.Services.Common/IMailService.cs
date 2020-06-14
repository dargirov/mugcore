using MugStore.Data.Models;
using System.Threading.Tasks;

namespace MugStore.Services.Common
{
    public interface IMailService
    {
        Task SendMailAsync(Order order);
    }
}

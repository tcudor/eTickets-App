using eTickets.Models;
using System.Threading.Tasks;
using eTickets.Data.Base;
using eTickets.Models;

namespace eTickets.Data.Services
{
    public interface ICinemasService : IEntityBaseRepository<Cinema>
    {
    }
}

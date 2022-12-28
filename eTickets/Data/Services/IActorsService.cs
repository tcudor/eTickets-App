using eTickets.Models;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public interface IActorsService
    {
        Task <IEnumerable<Actor>> GetAll();
        Actor GetById(int id);
        void Add(Actor actor);  
        void Update(int id,Actor newActor);
        void Delete(int id);
    }
}

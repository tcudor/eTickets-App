using eTickets.Data.Base;
using eTickets.Models;

namespace eTickets.Data.Services
{

    public class ItemsService : EntityBaseRepository<Item>, IItemsService
    {
        public ItemsService(AppDbContext context) : base(context) { }
    }
}

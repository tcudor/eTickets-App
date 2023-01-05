using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using eTickets.Data.Base;

namespace eTickets.Data.Services
{
    public class CinemasService : EntityBaseRepository<Cinema>, ICinemasService
    {
        public CinemasService(AppDbContext context) : base(context) { }
    }
}

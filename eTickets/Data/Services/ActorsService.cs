﻿using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using eTickets.Data.Base;

namespace eTickets.Data.Services
{
    public class ActorsService : EntityBaseRepository<Actor>, IActorsService
    {
        public ActorsService(AppDbContext context) : base(context) { }
    }
}

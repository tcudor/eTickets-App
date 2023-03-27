using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace eTickets.Data.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly AppDbContext _context;
        public OrdersService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole)
        {
            var orders = await _context.Orders.Include(n => n.OrderMovies).ThenInclude(n => n.Movie).Include(n => n.User).ToListAsync();
            if(userRole== "Admin") {
                orders=orders.Where(n => n.Username == userId).ToList();
            }
            return orders;
        }

        public async Task StoreOrderAsync(List<ShoppingCartMovie> items, string userId, string userEmailAddress)
        {
                var order = new Order()
                {
                    Username = userId,
                    Email = userEmailAddress
                };
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                foreach (var item in items)
                {
                    var OrderMovie = new OrderMovie()
                    {
                        Amount = item.Amount,
                        MovieId = item.Movie.Id,
                        OrderId = order.Id,
                        Price = item.Movie.Price
                    };

 
                    await _context.OrderMovies.AddAsync(OrderMovie);             
                }
                await _context.SaveChangesAsync();
            }
        }
    }

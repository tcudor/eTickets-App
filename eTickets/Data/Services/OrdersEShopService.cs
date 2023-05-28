using eTickets.Data.Static;
using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public class OrdersEShopService:IOrdersEShopService
    {
        private readonly AppDbContext _context;
        public OrdersEShopService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderEShop>> GetOrdersByUserIdAndRoleAsync(string userId,string userRole)
        {
            var orders = await _context.OrdersEShop.Include(n => n.OrderItems).ThenInclude(n => n.Item).Include(n => n.User).ToListAsync();
            if (userRole != "Admin")
            {
                orders = orders.Where(n => n.Username == userId).ToList();
            }
            return orders;
        }

        public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress)
        {
            var orderEShop = new OrderEShop()
            {
                Username = userId,
                Email = userEmailAddress
            };
            await _context.OrdersEShop.AddAsync(orderEShop);
            await _context.SaveChangesAsync();

            foreach (var item in items)
            {
                var OrderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    ItemId = item.Item.Id,
                    OrderEShopId = orderEShop.Id,
                    Price = item.Item.Price
                };


                await _context.OrderItems.AddAsync(OrderItem);
            }
            await _context.SaveChangesAsync();
        }
    }
}


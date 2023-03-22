using eTickets.Models;

namespace eTickets.Data.Services
{
    public interface IOrdersEShopService
    {
        Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress);
        Task<List<OrderEShop>> GetOrdersByUserIdAsync(string userId);
    }
}

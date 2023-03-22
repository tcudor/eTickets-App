using eTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data.Cart
{
    public class ShoppingCartItem
    {
        public AppDbContext _context { get; set; }

        public string ShoppingCartId { get; set; }

        public List<Models.ShoppingCartItem> ShoppingCartItems { get; set; }
        public ShoppingCartItem(AppDbContext context)
        {
            _context = context; 
        }

        public static ShoppingCartItem GetShoppingCart(IServiceProvider services)
        {
            ISession session=services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context=services.GetService<AppDbContext>();
            string cartId=session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);

            return new ShoppingCartItem(context) { ShoppingCartId = cartId };
        }

        public void AddItemToCart(Item item)
        {
            var shoppingCarItem = _context.ShoppingCartItems.FirstOrDefault(n => n.Item.Id == item.Id && n.ShoppingCartId == ShoppingCartId);
            if (shoppingCarItem == null) 
            {
                shoppingCarItem = new Models.ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    Item = item,
                    Amount = 1
                };
                _context.ShoppingCartItems.Add(shoppingCarItem);
            }
            else
            {
                shoppingCarItem.Amount++;
            }
            _context.SaveChanges();
        }

        public void RemoveItemFromCart(Item item)
        {
            var shoppingCarItem = _context.ShoppingCartItems.FirstOrDefault(n => n.Item.Id == item.Id && n.ShoppingCartId == ShoppingCartId);
            if (shoppingCarItem != null)
            {
                if(shoppingCarItem.Amount > 1)
                {
                    shoppingCarItem.Amount--;   
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCarItem);
                }
            } 
            _context.SaveChanges();
        }
        public List<Models.ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ?? (ShoppingCartItems=_context.ShoppingCartItems.Where(n=>n.ShoppingCartId==ShoppingCartId).Include(n=>n.Item).ToList());
        }

        public double GetShoppingCartTotal()
        {
            var total=_context.ShoppingCartItems.Where(n=>n.ShoppingCartId==ShoppingCartId).Select(n=>n.Item.Price*n.Amount).Sum();
            return total;
        }

        public async Task ClearShoppingCartAsync()
        {
            var items = await _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync();
            _context.ShoppingCartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        
    }
}

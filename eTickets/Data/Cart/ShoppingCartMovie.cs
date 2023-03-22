using eTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data.Cart
{
    public class ShoppingCartMovie
    {
        public AppDbContext _context { get; set; }

        public string ShoppingCartId { get; set; }

        public List<Models.ShoppingCartMovie> ShoppingCartMovies { get; set; }
        public ShoppingCartMovie(AppDbContext context)
        {
            _context = context; 
        }

        public static ShoppingCartMovie GetShoppingCart(IServiceProvider services)
        {
            ISession session=services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context=services.GetService<AppDbContext>();
            string cartId=session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);

            return new ShoppingCartMovie(context) { ShoppingCartId = cartId };
        }

        public void AddItemToCart(Movie movie)
        {
            var shoppingCarItem = _context.ShoppingCartMovies.FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);
            if (shoppingCarItem == null) 
            {
                shoppingCarItem = new Models.ShoppingCartMovie()
                {
                    ShoppingCartId = ShoppingCartId,
                    Movie = movie,
                    Amount = 1
                };
                _context.ShoppingCartMovies.Add(shoppingCarItem);
            }
            else
            {
                shoppingCarItem.Amount++;
            }
            _context.SaveChanges();
        }

        public void RemoveItemFromCart(Movie movie)
        {
            var shoppingCarItem = _context.ShoppingCartMovies.FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);
            if (shoppingCarItem != null)
            {
                if(shoppingCarItem.Amount > 1)
                {
                    shoppingCarItem.Amount--;   
                }
                else
                {
                    _context.ShoppingCartMovies.Remove(shoppingCarItem);
                }
            } 
            _context.SaveChanges();
        }
        public List<Models.ShoppingCartMovie> GetShoppingCartMovies()
        {
            return ShoppingCartMovies ?? (ShoppingCartMovies=_context.ShoppingCartMovies.Where(n=>n.ShoppingCartId==ShoppingCartId).Include(n=>n.Movie).ToList());
        }

        public double GetShoppingCartTotal()
        {
            var total=_context.ShoppingCartMovies.Where(n=>n.ShoppingCartId==ShoppingCartId).Select(n=>n.Movie.Price*n.Amount).Sum();
            return total;
        }

        public async Task ClearShoppingCartAsync()
        {
            var items = await _context.ShoppingCartMovies.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync();
            _context.ShoppingCartMovies.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}

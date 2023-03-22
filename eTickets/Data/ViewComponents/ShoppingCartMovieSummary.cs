using eTickets.Data.Cart;
using Microsoft.AspNetCore.Mvc;

namespace eTickets.Data.ViewComponents
{
     
    public class ShoppingCartMovieSummary:ViewComponent
    {
        private readonly ShoppingCartMovie _shoppingCart;
        public ShoppingCartMovieSummary(ShoppingCartMovie shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public IViewComponentResult Invoke()
        {
            var items=_shoppingCart.GetShoppingCartMovies();
            return View(items.Count);
        }
    }
}

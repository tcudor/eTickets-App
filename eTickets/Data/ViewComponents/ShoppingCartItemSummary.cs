using eTickets.Data.Cart;
using Microsoft.AspNetCore.Mvc;

namespace eTickets.Data.ViewComponents
{

    public class ShoppingCartItemSummary : ViewComponent
    {
        private readonly ShoppingCartItem _shoppingCart;
        public ShoppingCartItemSummary(ShoppingCartItem shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public IViewComponentResult Invoke()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            return View(items.Count);
        }
    }
}

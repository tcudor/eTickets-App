using eTickets.Data.Cart;
using eTickets.Data.Services;
using eTickets.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace eTickets.Controllers
{
    public class OrdersEShopController : Controller
    {
        private readonly IItemsService _itemsService;
        private readonly ShoppingCartItem _shoppingCart;
        private readonly IOrdersEShopService _ordersEShopService;

        public OrdersEShopController(IItemsService itemsService, ShoppingCartItem shoppingCart, IOrdersEShopService ordersService)
        {
            _itemsService = itemsService;
            _shoppingCart = shoppingCart;
            _ordersEShopService = ordersService;
        }

        public IActionResult ShoppingCart()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;
            var response = new ShoppingCartItemVM()
            {
                ShoppingCartItem = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(response);
        }
        public async Task<IActionResult> AddItemShoppingCart(int id)
        {
            var item = await _itemsService.GetByIdAsync(id);
            if (item != null)
            {
                _shoppingCart.AddItemToCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }
        public async Task<IActionResult> RemoveItemShoppingCart(int id)
        {
            var item = await _itemsService.GetByIdAsync(id);
            if (item != null)
            {
                _shoppingCart.RemoveItemFromCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }


        public async Task<IActionResult> CompleteOrder()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            string userId = "";
            string userEmailAddress = "";

            await _ordersEShopService.StoreOrderAsync(items, userId, userEmailAddress);
            await _shoppingCart.ClearShoppingCartAsync();

            return View("OrderCompleted");
        }

        public async Task<IActionResult> Index()
        {
            string userId = "";

            var orders = await _ordersEShopService.GetOrdersByUserIdAsync(userId);
            return View(orders);
        }
    }
}
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
    public class OrdersController : Controller
    {
        private readonly IMoviesService _moviesService;
        private readonly ShoppingCartMovie _shoppingCart;
        private readonly IOrdersService _ordersService;

        public OrdersController(IMoviesService moviesService, ShoppingCartMovie shoppingCart, IOrdersService ordersService)
        {
            _moviesService = moviesService;
            _shoppingCart = shoppingCart;
            _ordersService = ordersService;
        }

        public IActionResult ShoppingCart()
        {
            var items = _shoppingCart.GetShoppingCartMovies();
            _shoppingCart.ShoppingCartMovies = items;
            var response = new ShoppingCartMovieVM()
            {
                ShoppingCartMovie = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(response);
        }

        //public IActionResult ShoppingCartCupon()
        //{
        //    var items = _shoppingCart.GetShoppingCartMovies();
        //    _shoppingCart.ShoppingCartMovies = items;
        //    var response = new ShoppingCartMovieVM()
        //    {
        //        ShoppingCartMovie = _shoppingCart,
        //        ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal() / 1.1
        //    };
        //    return View(response);
        //}

        //public IActionResult Cupon()
        //{
        //    string primit_ca_parametru = "10%";
        //    string s = "10%";
        //    if (primit_ca_parametru == s)
        //    {
        //        return RedirectToAction(nameof(ShoppingCartCupon));
        //    }
        //    return RedirectToAction(nameof(ShoppingCart));

        //}

        public async Task<IActionResult> AddItemShoppingCart(int id)
        {
            var item = await _moviesService.GetMovieByIdAsync(id);
            if (item != null)
            {
                _shoppingCart.AddItemToCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }
        public async Task<IActionResult> RemoveItemShoppingCart(int id)
        {
            var item = await _moviesService.GetMovieByIdAsync(id);
            if (item != null)
            {
                _shoppingCart.RemoveItemFromCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

      
        public async Task<IActionResult> CompleteOrder()
        {
            var items = _shoppingCart.GetShoppingCartMovies();
            string userId = "";
            string userEmailAddress = "";

            await _ordersService.StoreOrderAsync(items, userId, userEmailAddress);
            await _shoppingCart.ClearShoppingCartAsync();

            return View("OrderCompleted");
        }

        public async Task<IActionResult> Index()
        {
            string userId = "";

            var orders = await _ordersService.GetOrdersByUserIdAsync(userId);
            return View(orders);
        }
    }
}
using eTickets.Data.Cart;
using eTickets.Data.Services;
using eTickets.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Net;

namespace eTickets.Controllers
{
    [Authorize]
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
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

            await _ordersService.StoreOrderAsync(items, userId, userEmailAddress);
            await _shoppingCart.ClearShoppingCartAsync();

            string toAddress = userEmailAddress;
            string subject = "Order Confimation!";
            string body = "Dear " + GenerateUsername(userEmailAddress) + ",\n\nThank you for ordering on our eCommerce website!";
            SendEmail(toAddress, subject, body);

            return View("OrderCompleted");
        }

        public static string GenerateUsername(string emailAddress)
        {
            int index = emailAddress.IndexOf('@');

            if (index != -1)
            {
                string username = emailAddress.Substring(0, index);
                return username;
            }

            return null; // În cazul în care adresa de email nu conține caracterul '@'
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(userId, userRole);
            return View(orders);
        }

        public void SendEmail(string toAddress, string subject, string body)
        {

            // Set the SMTP server details
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("tudorcristian.gheorghita@ulbsibiu.ro", "jqmifsfdshkupzlu");

            // Create a message to send
            MailMessage message = new MailMessage();
            message.From = new MailAddress("tudorcristian.gheorghita@ulbsibiu.ro");
            message.To.Add(toAddress);
            message.Subject = subject;
            message.Body = body;

            // Send the message
            client.Send(message);
        }
    }
}
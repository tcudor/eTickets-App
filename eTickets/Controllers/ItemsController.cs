using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Data.Static;
using eTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace eTickets.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class ItemsController : Controller
    {
        private readonly IItemsService _service;

        public ItemsController(IItemsService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var allItems = await _service.GetAllAsync();
            return View(allItems);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Home()
        {
            var allItems = await _service.GetAllAsync();
            return View(allItems);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("PictureUrl,Name,Description,Price")] Item item)
        {
            //var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (!ModelState.IsValid)
            {
                return View(item);
            }
            await _service.AddAsync(item);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var itemDetails = await _service.GetByIdAsync(id);

            if (itemDetails == null) return View("NotFound");
            return View(itemDetails);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var itemDetails = await _service.GetByIdAsync(id);

            if (itemDetails == null) return View("NotFound");
            return View(itemDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PictureUrl,Name,Description,Price")] Item item)
        {
            //var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (!ModelState.IsValid)
            {
                return View(item);
            }
            await _service.UpdateAsync(id, item);
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Delete(int id)
        {
            var itemDetails = await _service.GetByIdAsync(id);

            if (itemDetails == null) return View("NotFound");
            return View(itemDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemDetails = await _service.GetByIdAsync(id);

            if (itemDetails == null) return View("NotFound");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

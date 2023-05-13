using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Data.Static;
using eTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class MoviesController : Controller
    {
        private readonly IMoviesService _service;
        public MoviesController(IMoviesService service)
        {
            _service = service;
        }


        [AllowAnonymous]
        public async Task<IActionResult> Info()
        {
            return View("Info");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allMovies = await _service.GetAllAsync(n=>n.Cinema);
            return View(allMovies);
        }

        public async Task<IActionResult> AllMovies()
        {
            var allMovies = await _service.GetAllAsync(n => n.Cinema);
            return View(allMovies);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Search(string searchString)
        {
            var allMovies = await _service.GetAllAsync(n => n.Cinema);

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResultNew = allMovies.Where(n => n.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase) || n.Description.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

                return View("Index", filteredResultNew);
            }

            return View("Index", allMovies);
        }
        [AllowAnonymous]
        public async Task<IActionResult>Details(int id)
        {
            var movieDetails = await _service.GetMovieByIdAsync(id);
            return View(movieDetails);
        }


        public async Task<IActionResult> Create()
        {   
            
            var movieDropdownsData = await _service.GetNewMovieDropdownsValues();            
            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewMovieVM movie)
        {
            if(!ModelState.IsValid)
            {
                var movieDropdownsData = await _service.GetNewMovieDropdownsValues();
                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
                return View(movie);
            }
            
            await _service.AddNewMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var movieDetails = await _service.GetMovieByIdAsync(id);
            if(movieDetails== null) return View("NotFound");

            var response = new NewMovieVM()
            {
                Id=movieDetails.Id,
                Name=movieDetails.Name,
                Description=movieDetails.Description,
                Price=movieDetails.Price,
                StartDate=movieDetails.StartDate,
                EndDate=movieDetails.EndDate,
                ImageURL=movieDetails.ImageURL,
                MovieCategory=movieDetails.MovieCategory,
                CinemaId=movieDetails.CinemaId,
                ProducerId=movieDetails.ProducerId,
                ActorId=movieDetails.Actors_Movies.Select(n=>n.ActorId).ToList()
            };

            var movieDropdownsData = await _service.GetNewMovieDropdownsValues();
            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewMovieVM movie)
        {
            if (id != movie.Id) return View("NotFound");
            if (!ModelState.IsValid)
            {
                var movieDropdownsData = await _service.GetNewMovieDropdownsValues();
                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
                return View(movie);
            }

            await _service.UpdateMovieAsync(movie);
            return RedirectToAction("AllMovies");
        }



        public async Task<IActionResult> Delete(int id)
        {
            var movieDetails = await _service.GetByIdAsync(id);

            if (movieDetails == null) return View("NotFound");
            return View(movieDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id, NewMovieVM movie)
        {
            var movieDetails = await _service.GetByIdAsync(id);
            if (id != movie.Id) return View("NotFound");
            await _service.DeleteMovieAsync(movie);
            return View("Control");
        }
    }
}

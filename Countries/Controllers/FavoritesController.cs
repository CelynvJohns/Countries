using Countries.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Countries.Utilities;

namespace Countries.Controllers
{
    public class FavoritesController : Controller
    {
        public ActionResult Index()
        {
            var favoriteCountries = HttpContext.Session.Get<List<Country>>("Favorites") ?? new List<Country>();
            return View(favoriteCountries);
        }

        [HttpPost] // This attribute is necessary for handling POST requests
        public ActionResult AddToFavorite(string name)
        {
            try
            {
                if (HttpContext != null && HttpContext.Session != null)
                {
                    var favoriteCountries = HttpContext.Session.Get<List<Country>>("Favorites") ?? new List<Country>();

                    // Add the selected country to favorites
                    var countryToAdd = new Country { Name = name }; // Create a new Country object with the name
                    favoriteCountries.Add(countryToAdd);
                    HttpContext.Session.Set("Favorites", favoriteCountries);

                    // Redirect to the Favorites page
                    return RedirectToAction("Index");
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);  // Log the exception to the console
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult RemoveFromFavorite(string name)
        {
            try
            {
                Console.WriteLine($"Attempting to remove {name} from favorites.");

                if (HttpContext != null && HttpContext.Session != null)
                {
                    var favoriteCountries = HttpContext.Session.Get<List<Country>>("Favorites") ?? new List<Country>();

                    // Remove the country from favorites (case-insensitive comparison)
                    var countryToRemove = favoriteCountries.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
                    if (countryToRemove != null)
                    {
                        favoriteCountries.Remove(countryToRemove);
                        HttpContext.Session.Set("Favorites", favoriteCountries);
                        Console.WriteLine($"Successfully removed {name} from favorites.");
                    }
                    else
                    {
                        Console.WriteLine($"{name} not found in favorites.");
                    }

                    // Return a success response
                    return Json(new { success = true });
                }

                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing {name} from favorites: {ex.Message}");
                return Json(new { success = false, error = ex.Message });
            }
        }


        [HttpPost]
        public ActionResult ClearFavorites()
        {
            try
            {
                HttpContext.Session.Set("Favorites", new List<Country>());
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing favorites: {ex.Message}");
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}

using Countries.Models;
using Microsoft.AspNetCore.Mvc;
using Countries.Utilities; // Add the appropriate namespace for your SessionHelper
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Countries.ViewModels;


namespace Countries.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public HomeController(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        private static List<Country> countries = new List<Country>
        {
        new Country { Name = "Canada", Game = "Winter Olympics", Category = "Curling/Indoor" },
         new Country { Name = "Sweden", Game = "Winter Olympics", Category = "Curling/Indoor" },
          new Country { Name = "Great Britain", Game = "Winter Olympics", Category = "Curling/Indoor" },
           new Country { Name = "Jamacia", Game = "Winter Olympics", Category = "Bobsleigh/Outdoor" },
            new Country { Name = "Italy", Game = "Winter Olympics", Category = "Bobsleigh/Outdoor" },
             new Country { Name = "Japan", Game = "Winter Olympics", Category = "Bobsleigh/Outdoor" },
              new Country { Name = "Germany", Game = "Summer Olympics", Category = "Diving/Indoor" },
              new Country { Name = "China", Game = "Summer Olympics", Category = "Diving/Indoor" },
              new Country { Name = "Mexico", Game = "Summer Olympics", Category = "Diving/Indoor" },
              new Country { Name = "Brazil", Game = "Summer Olympics", Category = "Road Cycling/Outdoor" },
              new Country { Name = "Netherlands", Game = "Summer Olympics", Category = "Road Cycling/Outdoor" },
              new Country { Name = "USA", Game = "Summer Olympics", Category = "Road Cycling/Outdoor" },
              new Country { Name = "Thailand", Game = "Paralmics", Category = "Archery/Indoor" },
              new Country { Name = "Uruguay", Game = "Paralympics", Category = "Archery/Indoor" },
              new Country { Name = "Ukraine", Game = "Paralympics", Category = "Archery/Indoor" },
              new Country { Name = "Austria", Game = "Paralympics", Category = "Canoe Spring/Outdoor" },
              new Country { Name = "Pakistan", Game = "Paralympics", Category = "Canoe Spring/Outdoor" },
              new Country { Name = "Zimbabwe", Game = "Paralympics", Category = "Canoe Spring/Outdoor" },
              new Country { Name = "France", Game = "Youth Olympic Games", Category = "Breakdancing/Indoor" },
              new Country { Name = "Cyprus", Game = "Youth Olympic Games", Category = "Breakdancing/Indoor" },
              new Country { Name = "Russia", Game = "Youth Olympic Games", Category = "Breakdancing/Indoor" },
              new Country { Name = "Finland", Game = "Youth Olympic Games", Category = "Skateboarding/Outdoor" },
              new Country { Name = "Slovakia", Game = "Youth Olympic Games", Category = "Skateboarding/Outdoor" },
              new Country { Name = "Portugal", Game = "Youth Olympic Games", Category = "Skateboarding/Outdoor" }
    };

        public ActionResult Index(OlympicGamesViewModel viewModel)
        {
            try
            {
                // Retrieve data from your data source or any other logic based on the view model
                var allCountries = countries.ToList();

                // Filter countries based on the values provided in the view model
                var filteredCountries = FilterCountries(allCountries, viewModel.Game, viewModel.Category, viewModel.SelectedCountryNames);

                // Create an instance of OlympicGamesViewModel and populate its properties
                var olympicGamesViewModel = new OlympicGamesViewModel
                {
                    Game = viewModel.Game,
                    Category = viewModel.Category,
                    SelectedCountryNames = viewModel.SelectedCountryNames,
                    Countries = filteredCountries.Any() ? filteredCountries : allCountries
                };

                return View("Index", olympicGamesViewModel); // Pass the view model to the view
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Index action: {ex.Message}");
                return View("Error"); // Direct to an error view in case of an exception
            }
        }


        // Helper method to filter countries based on the view model
        private static List<Country> FilterCountries(List<Country> allCountries, string game, string category, List<string> selectedCountries)
        {
            var filteredCountries = allCountries;

            // Filter by game
            if (!string.IsNullOrEmpty(game))
            {
                filteredCountries = filteredCountries.Where(c => c.Game == game).ToList();
            }

            // Filter by category
            if (!string.IsNullOrEmpty(category))
            {
                filteredCountries = filteredCountries.Where(c => c.Category == category).ToList();
            }

            // Filter by selected countries
            if (selectedCountries != null && selectedCountries.Any())
            {
                filteredCountries = filteredCountries.Where(c => selectedCountries.Contains(c.Name)).ToList();
            }

            return filteredCountries;
        }


        public ActionResult AddToFavorite(string name)
        {
            try
            {
                if (HttpContext != null && HttpContext.Session != null)
                {
                    var favoriteCountries = HttpContext.Session.Get<List<Country>>("Favorites") ?? new List<Country>();

                    // Add the selected country to favorites
                    var countryToAdd = countries.FirstOrDefault(c => c.Name == name);
                    if (countryToAdd != null)
                    {
                        favoriteCountries.Add(countryToAdd);

                        // Save the updated favorites list to the session
                        HttpContext.Session.Set("Favorites", favoriteCountries);
                    }

                    // Redirect to the Favorites page
                    return RedirectToAction("Index", "Home", new { area = "" });
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);  // Log the exception to the console
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}
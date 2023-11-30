// ViewModel for Olympic Games
using Countries.Models;
using System.Collections.Generic;

namespace Countries.ViewModels
{
    public class OlympicGamesViewModel
    {
        public string Game { get; set; }
        public string Category { get; set; }
        public List<string> SelectedCountryNames { get; set; }
        public List<Country> Countries { get; set; } // Add this property
    }
}

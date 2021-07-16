using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MugStore.Data.Models;
using MugStore.Services.Data;
using MugStore.Web.Areas.Admin.ViewModels.City;
using MugStore.Web.Controllers;
using System.IO;
using System.Linq;

namespace MugStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "LoggedIn")]
    public class CityController : BaseController
    {
        private readonly ICitiesService cities;
        private readonly IWebHostEnvironment hostingEnvironment;

        public CityController(ICitiesService cities, IWebHostEnvironment hostingEnvironment)
        {
            this.cities = cities;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var cities = this.cities.Get().ToList();
            var viewModel = new IndexViewModel()
            {
                Cities = cities
            };

            return this.View(viewModel);
        }

        public IActionResult Import()
        {
            var path = Path.Combine(this.hostingEnvironment.ContentRootPath, "AppData");
            using (var reader = new StreamReader(path + @"\speedy_sites.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Replace("\"", string.Empty);
                    var values = line.Split(',');
                    var cityType = CityType.City;
                    if (values[6] == "с.")
                    {
                        cityType = CityType.Village;
                    }

                    var city = new City()
                    {
                        Name = this.UppercaseFirst(values[3]),
                        Type = cityType,
                        Highlight = false,
                        PostCode = int.Parse(values[4])
                    };

                    this.cities.Create(city);
                }
            }

            return this.RedirectToAction("Index", "City");
        }

        public IActionResult EditHighlight(int id)
        {
            var city = this.cities.Get(id);
            if (city == null)
            {
                return NotFound();
            }

            city.Highlight = !city.Highlight;
            this.cities.Save();

            return this.RedirectToAction("Index", "City");
        }

        private string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            return char.ToUpper(s[0]) + s.ToLower().Substring(1);
        }
    }
}

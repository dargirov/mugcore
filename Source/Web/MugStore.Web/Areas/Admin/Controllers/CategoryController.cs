using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MugStore.Data.Models;
using MugStore.Services.Data;
using MugStore.Web.Areas.Admin.ViewModels.Category;
using MugStore.Web.Controllers;
using System.Linq;

namespace MugStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "LoggedIn")]
    public class CategoryController : BaseController
    {
        private readonly ICategoriesService categories;

        public CategoryController(ICategoriesService categories)
        {
            this.categories = categories;
        }

        public IActionResult Index()
        {
            var categories = this.categories.Get().OrderBy(c => c.Order).ToList();

            var viewModel = new IndexViewModel()
            {
                Categories = categories
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            var category = this.categories.Get().Where(t => t.Name == model.Name || t.Acronym == model.Acronym).FirstOrDefault();
            if (category == null)
            {
                var newCategory = new Category()
                {
                    Name = model.Name,
                    Acronym = model.Acronym,
                    Order = model.Order,
                    Active = true
                };

                this.categories.Create(newCategory);
            }

            return this.RedirectToAction("Index", "Category");
        }
    }
}

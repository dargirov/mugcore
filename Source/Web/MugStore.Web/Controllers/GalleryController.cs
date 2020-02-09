using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MugStore.Common;
using MugStore.Services.Data;
using MugStore.Web.ViewModels.Gallery;
using System;
using System.Linq;

namespace MugStore.Web.Controllers
{
    public class GalleryController : BaseController
    {
        private readonly ICategoriesService categories;
        private readonly IProductsService products;
        private readonly ITagsService tags;

        public GalleryController(ICategoriesService categories, IProductsService products, ITagsService tags)
        {
            this.categories = categories;
            this.products = products;
            this.tags = tags;
        }

        public IActionResult Index(int id = 1)
        {
            var page = id;
            var categories = this.categories.Get().OrderBy(c => c.Order).ToList();
            var totalCount = this.products.Get().Where(p => p.Active).Count();

            var itemsPerPage = GlobalConstants.ProductsPerPage;
            var totalPages = (int)Math.Ceiling(totalCount / (decimal)itemsPerPage);
            var itemsToSkip = (page - 1) * itemsPerPage;

            var products = this.products.Get()
                .Where(p => p.Active)
                .Include(p => p.Images)
                .OrderByDescending(p => p.Id)
                .Skip(itemsToSkip)
                .Take(itemsPerPage)
                .ToList();

            this.AddTagsToViewBag(this.tags);
            this.ViewBag.PageDescription = "Колекция от чаши с картинки в 3D изглед.";

            var viewModel = new IndexViewModel()
            {
                Categories = categories,
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                BaseUrl = GetBaseUrl()
            };

            return this.View(viewModel);
        }

        public IActionResult Category(string acronym)
        {
            var category = this.categories.Get(acronym);
            if (category == null)
            {
                return NotFound();
            }

            var categories = this.categories.Get().OrderBy(c => c.Order).ToList();
            var products = this.products.Get().Where(p => p.Active && p.CategoryId == category.Id).Include(p => p.Images).OrderByDescending(p => p.Id).ToList();
            this.AddTagsToViewBag(this.tags);
            this.ViewBag.PageDescription = $"Колекция от чаши със снимки за {category.Name} в 3D изглед.";

            var viewModel = new IndexViewModel()
            {
                CategoryName = category.Name,
                Categories = categories,
                Products = products,
                Acronym = acronym,
                BaseUrl = GetBaseUrl(),
                Type = "gallery"
            };

            return this.View("Index", viewModel);
        }

        public IActionResult Tag(string acronym)
        {
            var tag = this.tags.GetProductTag(acronym);
            if (tag == null)
            {
                return NotFound(acronym);
            }

            this.AddTagsToViewBag(this.tags);
            this.ViewBag.PageDescription = $"Колекция от чаши със снимки за {tag.Name} в 3D изглед.";
            var products = tag.Products.Select(x => x.Product).ToList();
            var categories = this.categories.Get().OrderBy(c => c.Order).ToList();

            var viewModel = new IndexViewModel()
            {
                CategoryName = tag.Name,
                Products = products,
                Categories = categories,
                Acronym = acronym,
                BaseUrl = GetBaseUrl(),
                Type = "tag"
            };

            return this.View("Index", viewModel);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MugStore.Common;
using MugStore.Data.Models;
using MugStore.Services.Common;
using MugStore.Services.Data;
using MugStore.Web.Areas.Admin.ViewModels.Product;
using MugStore.Web.Controllers;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MugStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "LoggedIn")]
    public class ProductController : BaseController
    {
        private readonly ICategoriesService categories;
        private readonly IProductsService products;
        private readonly ITagsService tags;
        private readonly IWebHostEnvironment hostingEnvironment;

        public ProductController(ICategoriesService categories, IProductsService products, ITagsService tags, IWebHostEnvironment hostingEnvironment)
        {
            this.categories = categories;
            this.products = products;
            this.tags = tags;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var products = this.products.Get().OrderByDescending(p => p.Id).ToList();
            var model = new IndexViewModel()
            {
                Products = products
            };

            return this.View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = this.products.Get(id);
            if (product == null)
            {
                return NotFound(id.ToString());
            }

            var viewModel = this.Mapper.Map<CreateViewModel>(product);
            viewModel.Categories = this.categories.Get();
            viewModel.AllTags = this.tags.GetProductTag().Where(t => t.Active).ToList();

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CreateViewModel model)
        {
            model.Categories = this.categories.Get();

            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Edit", "Product", new { id = id });
            }

            var product = this.products.Get(id);
            if (product == null)
            {
                return NotFound(id.ToString());
            }

            product.Title = model.Title;
            product.PageTitle = model.PageTitle;
            product.Description = model.Description;
            product.Code = model.Code;
            product.PreviewData = model.PreviewData;
            product.Active = model.Active;
            product.CategoryId = model.CategoryId;
            product.Acronym = model.Acronym;
            product.LinkToSource = model.LinkToSource;
            this.products.Save();

            return this.RedirectToAction("Edit", "Product", new { id = id });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new CreateViewModel()
            {
                Categories = this.categories.Get()
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var product = new Product()
            {
                Active = model.Active,
                PageTitle = model.PageTitle,
                CategoryId = model.CategoryId,
                Code = model.Code,
                Description = model.Description,
                Title = model.Title,
                PreviewData = model.PreviewData,
                Acronym = model.Acronym,
                LinkToSource = model.LinkToSource
            };

            this.products.Create(product);
            model.Categories = this.categories.Get();

            return this.RedirectToAction("Edit", "Product", new { id = product.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(int id, IFormFile file)
        {
            var product = this.products.Get(id);
            if (product == null)
            {
                return NotFound(id.ToString());
            }

            if (file.Length > 0)
            {
                var imagesPath = Path.Combine(this.hostingEnvironment.ContentRootPath, GlobalConstants.PathToGalleryImages.FixOsPath());
                var path = imagesPath + id.ToString();

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var name = Guid.NewGuid().ToString();
                var type = file.ContentType.Split('/');
                var filePath = path + @"\" + name + "." + type[1];

                using (var stream = new FileStream(filePath.FixOsPath(), FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var image = new ProductImage()
                {
                    Name = name,
                    OriginalName = file.FileName,
                    Path = id.ToString(),
                    ContentType = file.ContentType,
                    ProductId = id,
                    Preview3d = false,
                    Thumb = false
                };

                product.Images.Add(image);
                this.products.Save();
            }

            return this.RedirectToAction("Edit", "Product", new { area = "Admin", id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTag(int tagId, int productId, string type)
        {
            var product = this.products.Get(productId);
            var tag = this.tags.GetProductTag(tagId);

            if (product == null || tag == null)
            {
                return this.Json(new { success = false });
            }

            switch (type)
            {
                case "add":
                    product.Tags.Add(new ProductTagProduct() { Product = product, ProductTag = tag });
                    break;
                case "remove":
                    var productTagProduct = product.Tags.FirstOrDefault(x => x.ProductTagId == tagId);
                    product.Tags.Remove(productTagProduct);
                    break;
            }

            this.products.Save();

            return this.Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditImage(int productId, int imageId, string type, string imageType)
        {
            var product = this.products.Get(productId);
            if (product == null)
            {
                return this.Json(new { success = false });
            }

            ProductImage image = null;
            foreach (var item in product.Images)
            {
                if (item.Id == imageId)
                {
                    image = item;
                }
            }

            if (image == null)
            {
                return this.Json(new { success = false });
            }

            if (imageType == "preview")
            {
                switch (type)
                {
                    case "add":
                        {
                            var images = new List<object>();

                            if (product.PreviewData != null)
                            {
                                var jsonData = JsonSerializer.Deserialize<dynamic>(product.PreviewData);
                                images = jsonData.images.ToObject<List<object>>();
                            }

                            images.Add(new { id = imageId, name = image.Name, width = "full", height = "full" });

                            var data = new
                            {
                                version = 1,
                                images = images
                            };

                            product.PreviewData = JsonSerializer.Serialize(data);
                            image.Preview3d = true;
                            break;
                        }

                    case "remove":
                        {
                            var images = new List<object>();
                            var jsonData = JsonSerializer.Deserialize<dynamic>(product.PreviewData);
                            var imagesData = jsonData.images.ToObject<List<object>>();
                            foreach (var i in imagesData)
                            {
                                if (i.id != imageId)
                                {
                                    images.Add(new { id = i.id, name = i.name, width = i.width, height = i.height });
                                }
                            }

                            var data = new
                            {
                                version = 1,
                                images = images
                            };

                            product.PreviewData = JsonSerializer.Serialize(data);
                            image.Preview3d = false;
                            break;
                        }
                }
            }
            else
            {
                switch (type)
                {
                    case "add":
                        image.Thumb = true;
                        break;
                    case "remove":
                        image.Thumb = false;
                        break;
                }
            }

            this.products.Save();

            return this.Json(new { success = true });
        }
    }
}

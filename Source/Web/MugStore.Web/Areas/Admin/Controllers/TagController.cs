using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MugStore.Data.Models;
using MugStore.Services.Data;
using MugStore.Web.Areas.Admin.ViewModels.Tag;
using MugStore.Web.Controllers;
using System.Linq;

namespace MugStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "LoggedIn")]
    public class TagController : BaseController
    {
        private readonly ITagsService tags;

        public TagController(ITagsService tags)
        {
            this.tags = tags;
        }

        public IActionResult Index()
        {
            var productTags = this.tags.GetProductTag().ToList();
            var postTags = this.tags.GetPostTag().ToList();

            var viewModel = new IndexViewModel()
            {
                ProductTags = productTags,
                PostTags = postTags
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

            var tag = model.IsProductTag
                ? this.tags.GetProductTag().Any(t => t.Name == model.Name || t.Acronym == model.Acronym)
                : this.tags.GetPostTag().Any(t => t.Name == model.Name || t.Acronym == model.Acronym);

            if (!tag)
            {
                if (model.IsProductTag)
                {
                    var newTag = new ProductTag() { Name = model.Name, Acronym = model.Acronym, Active = true };
                    this.tags.Create(newTag);
                }
                else
                {
                    var newTag = new PostTag() { Name = model.Name, Acronym = model.Acronym, Active = true };
                    this.tags.Create(newTag);
                }
            }

            return this.RedirectToAction("Index", "Tag");
        }
    }
}

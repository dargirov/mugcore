using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MugStore.Data.Models;
using MugStore.Services.Data;
using MugStore.Web.Areas.Admin.ViewModels.Blog;
using MugStore.Web.Controllers;
using System.Linq;

namespace MugStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "LoggedIn")]
    public class BlogController : BaseController
    {
        private readonly IBlogService blog;
        private readonly ITagsService tags;

        public BlogController(IBlogService blog, ITagsService tags)
        {
            this.blog = blog;
            this.tags = tags;
        }

        public IActionResult Index()
        {
            var posts = this.blog.GetPosts(x => true).ToList();

            var viewModel = new IndexViewModel()
            {
                Posts = posts
            };

            return this.View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new CreateViewModel();

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

            var post = new Post()
            {
                Title = model.Title,
                PageTitle = model.PageTitle,
                Acronym = model.Acronym,
                Active = model.Active,
                ShortDescription = model.ShortDescription,
                FullDescription = model.FullDescription,
                PageDescription = model.PageDescription,
            };

            this.blog.Create(post);

            return this.RedirectToAction("Edit", "Blog", new { id = post.Id });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var post = this.blog.GetPost(id);
            if (post == null)
            {
                return BadRequest(id.ToString());
            }

            var viewModel = this.Mapper.Map<CreateViewModel>(post);
            viewModel.AllTags = this.tags.GetPostTag().Where(t => t.Active).ToList();

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CreateViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Edit", "Blog", new { id = id });
            }

            var post = this.blog.GetPost(id);
            if (post == null)
            {
                return BadRequest(id.ToString());
            }

            post.Title = model.Title;
            post.PageTitle = model.PageTitle;
            post.Acronym = model.Acronym;
            post.Active = model.Active;
            post.ShortDescription = model.ShortDescription;
            post.FullDescription = model.FullDescription;
            post.PageDescription = model.PageDescription;
            this.blog.Save();

            return this.RedirectToAction("Edit", "Blog", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTag(int tagId, int postId, string type)
        {
            var post = this.blog.GetPost(postId);
            var tag = this.tags.GetPostTag(tagId);

            if (post == null || tag == null)
            {
                return this.Json(new { success = false });
            }

            switch (type)
            {
                case "add":
                    post.Tags.Add(new PostTagPost() { Post = post, PostTag = tag });
                    break;
                case "remove":
                    var postTagPost = post.Tags.FirstOrDefault(x => x.PostTagId == tagId);
                    post.Tags.Remove(postTagPost);
                    break;
            }

            this.blog.Save();

            return this.Json(new { success = true });
        }
    }
}
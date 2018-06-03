using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MugStore.Services.Data;
using MugStore.Web.ViewModels.Blog;

namespace MugStore.Web.Controllers
{
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
            this.ViewBag.PageDescription = "Блог";
            this.AddTagsToViewBag(this.tags);

            var viewModel = new IndexViewModel()
            {
                Posts = this.blog.GetPosts(x => x.Active).ToList()
            };

            return this.View(viewModel);
        }

        public IActionResult Post(string acronym)
        {
            if (string.IsNullOrWhiteSpace(acronym))
            {
                return this.NotFound();
            }

            var post = this.blog.GetPost(acronym);
            if (post == null)
            {
                return this.NotFound();
            }

            this.ViewBag.PageDescription = post.PageDescription;
            this.AddTagsToViewBag(this.tags);
            var viewModel = this.Mapper.Map<PostViewModel>(post);

            return this.View(viewModel);
        }
    }
}
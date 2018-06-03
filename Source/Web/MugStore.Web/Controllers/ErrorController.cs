using Microsoft.AspNetCore.Mvc;
using MugStore.Data.Models;
using MugStore.Services.Common;
using MugStore.Services.Data;

namespace MugStore.Web.Controllers
{
    public class ErrorController : BaseController
    {
        private readonly ITagsService tags;
        private readonly ILoggerService logger;

        public ErrorController(ITagsService tags, ILoggerService logger)
        {
            this.tags = tags;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            this.AddTagsToViewBag(this.tags);
            this.ViewBag.PageDescription = "Грешка.";
            this.logger.Log(LogLevel.Error, "Error", "500");

            this.Response.StatusCode = 500;
            return this.View();
        }

        public IActionResult NotFound()
        {
            this.AddTagsToViewBag(this.tags);
            this.ViewBag.PageDescription = "Несъществуваща страница.";
            this.logger.Log(LogLevel.Warn, "NotFound", "404");

            this.Response.StatusCode = 404;
            return this.View();
        }
    }
}
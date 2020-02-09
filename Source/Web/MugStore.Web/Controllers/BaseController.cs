using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MugStore.Common;
using MugStore.Services.Data;
using MugStore.Web.Infrastructure.Mapping;
using System;
using System.Linq;

namespace MugStore.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IMapper Mapper => AutoMapperConfig.Configuration.CreateMapper();

        protected void AddTagsToViewBag(ITagsService tags)
        {
            this.ViewBag.Tags = tags.GetProductTag().Where(t => t.Active).OrderBy(t => Guid.NewGuid()).Take(GlobalConstants.MaxTagsInFooter).ToList();
        }

        protected string GetBaseUrl()
        {
            return $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host.Value}";
        }
    }
}

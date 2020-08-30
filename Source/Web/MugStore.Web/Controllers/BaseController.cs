using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MugStore.Common;
using MugStore.Services.Data;
using MugStore.Web.Infrastructure.Mapping;
using MugStore.Web.Models;

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
            //return $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host.Value}";
            return $"https://{this.HttpContext.Request.Host.Value}";
        }

        protected IEnumerable<ColorMugModel> GetColorMugs(IConfiguration configuration)
        {
            yield return ReadColorMug(configuration, "Red", "Червен");
            yield return ReadColorMug(configuration, "Blue", "Син");
            yield return ReadColorMug(configuration, "Green", "Зелен");
            yield return ReadColorMug(configuration, "Yellow", "Жълт");
        }

        private ColorMugModel ReadColorMug(IConfiguration configuration, string color, string name)
        {
            var enaled = Convert.ToBoolean(configuration.GetSection($"ColorMugs:{color}")["Enabled"]);
            var singleMugPrice = Convert.ToDecimal(configuration.GetSection($"ColorMugs:{color}")["SingleMugPrice"], CultureInfo.InvariantCulture);
            var singleMugMsrpPrice = Convert.ToDecimal(configuration.GetSection($"ColorMugs:{color}")["SingleMugMsrpPrice"], CultureInfo.InvariantCulture);
            var sungleMugPriceSupplier = Convert.ToDecimal(configuration.GetSection($"ColorMugs:{color}")["SungleMugPriceSupplier"], CultureInfo.InvariantCulture);
            return new ColorMugModel
            {
                Enabled = enaled,
                SingleMugPrice = singleMugPrice,
                SingleMugMsrpPrice = singleMugMsrpPrice,
                SungleMugPriceSupplier = sungleMugPriceSupplier,
                Name = name,
                Color = color,
            };
        }
    }
}

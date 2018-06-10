using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MugStore.Services.Data;
using System.Linq;
using MugStore.Web.ViewModels.Product;

namespace MugStore.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductsService products;
        private readonly ITagsService tags;
        private readonly ICitiesService cities;
        private readonly ICouriersService couriers;
        private readonly IConfiguration configuration;

        public ProductController(IProductsService products, ITagsService tags, ICitiesService cities, ICouriersService couriers, IConfiguration configuration)
        {
            this.products = products;
            this.tags = tags;
            this.cities = cities;
            this.couriers = couriers;
            this.configuration = configuration;
        }

        public IActionResult Index(string acronym)
        {
            var product = this.products.Get(acronym);
            if (product == null)
            {
                return NotFound();
            }

            this.ViewBag.Cities = this.cities.Get().Where(c => c.Highlight).OrderBy(x => x.Name).ToList();
            this.ViewBag.Couriers = this.couriers.Get().Where(c => c.Active).OrderBy(x => x.Name).ToList();
            this.ViewBag.ShowRight = false;
            this.ViewBag.PageHeading = product.Title;
            this.ViewBag.SingleMugPrice = decimal.Parse(this.configuration["AppSettings:SingleMugPrice"]);
            this.ViewBag.DeliveryPrice = decimal.Parse(this.configuration["AppSettings:DeliveryPrice"]);
            this.AddTagsToViewBag(this.tags);
            this.ViewBag.PageDescription = $"{product.PageTitle}, {product.Title}";
            var viewModel = this.Mapper.Map<IndexViewModel>(product);
            viewModel.Email = this.configuration["AppSettings:ContactsEmail"];
            viewModel.Phone = this.configuration["AppSettings:ContactsPhone"];

            return this.View(viewModel);
        }
    }
}

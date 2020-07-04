using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MugStore.Common;
using MugStore.Data.Models;
using MugStore.Services.Common;
using MugStore.Services.Data;
using MugStore.Web.ViewModels.Home;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace MugStore.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IProductsService products;
        private readonly ICitiesService cities;
        private readonly ITagsService tags;
        private readonly ICategoriesService categories;
        private readonly ICouriersService couriers;
        private readonly IFeedbacksService feedbacks;
        private readonly IOrdersService orders;
        private readonly IBulletinsService bulletins;
        private readonly IBlogService blog;
        private readonly IConfiguration configuration;
        private readonly ILoggerService logger;

        public HomeController(IProductsService products, ICitiesService cities, ITagsService tags, ICategoriesService categories, ICouriersService couriers, IFeedbacksService feedbacks, IOrdersService orders, IBulletinsService bulletins, IBlogService blog, IConfiguration configuration, ILoggerService loggerService)
        {
            this.products = products;
            this.cities = cities;
            this.tags = tags;
            this.categories = categories;
            this.couriers = couriers;
            this.feedbacks = feedbacks;
            this.orders = orders;
            this.bulletins = bulletins;
            this.blog = blog;
            this.configuration = configuration;
            this.logger = loggerService;
        }

        private IList<string> MugInfoTypes => new List<string>() { "dark", "white" };

        public IActionResult Index()
        {
            var products = this.products.Get().Where(c => c.Active).Include(p => p.Images).OrderByDescending(p => p.Id).Take(GlobalConstants.MaxProductsInHomePage).ToList();
            this.ViewBag.Cities = this.cities.Get().Where(c => c.Highlight).OrderBy(x => x.Name).ToList();
            this.ViewBag.Couriers = this.couriers.Get().Where(c => c.Active).OrderBy(c => c.Name).ToList();
            this.ViewBag.ShowRight = true;
            this.ViewBag.SingleMugPrice = decimal.Parse(configuration["AppSettings:SingleMugPrice"]);
            this.ViewBag.SingleMugMsrpPrice = decimal.Parse(configuration["AppSettings:SingleMugMsrpPrice"]);
            this.ViewBag.Decrease = Math.Round((this.ViewBag.SingleMugMsrpPrice - this.ViewBag.SingleMugPrice) / this.ViewBag.SingleMugMsrpPrice * 100);
            this.ViewBag.DeliveryPrice = decimal.Parse(configuration["AppSettings:DeliveryPrice"]);
            this.ViewBag.PageDescription = "С този сайт може сам да си направиш чаша. Качи до 3 снимки и ги разположи на желаното място върху 3D модел на чаша. Поръчката става бързо и не е необходима регистрация.";
            this.ViewBag.FashShippingEnabled = bool.Parse(this.configuration["AppSettings:FastShippingEnabled"]);
            this.ViewBag.VacationEnabled = bool.Parse(this.configuration["AppSettings:VacationEnabled"]);
            this.ViewBag.VacationMessage = this.configuration["AppSettings:VacationMessage"];
            this.ViewBag.EnabledColors = this.configuration.GetSection("EnabledColors");
            this.AddTagsToViewBag(this.tags);

            var viewModel = new IndexViewModel()
            {
                Products = products,
                MugInfoType = this.MugInfoTypes.OrderBy(x => Guid.NewGuid()).First(),
                BlogPosts = this.blog.GetPosts(x => x.Active).ToList()
            };

            return this.View(viewModel);
        }

        [HttpGet]
        public IActionResult Contacts()
        {
            this.AddTagsToViewBag(this.tags);
            this.ViewBag.PageDescription = "За контакти и въпроси при направа на чаша може да се свържете с нас.";

            var viewModel = new ContactsViewModel()
            {
                Email = configuration["AppSettings:ContactsEmail"],
                Phone = configuration["AppSettings:ContactsPhone"],
                SiteKey = configuration["AppSettings:ReCaptchaSiteKey"]
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contacts(ContactsInputModel model)
        {
            this.AddTagsToViewBag(this.tags);
            this.ViewBag.MailSend = false;
            this.ViewBag.PageDescription = "За контакти и въпроси при направа на чаша може да се свържете с нас.";
            var captcha = this.HttpContext.Request.Form["g-recaptcha-response"].FirstOrDefault();

            if (this.ModelState.IsValid && !string.IsNullOrWhiteSpace(captcha))
            {
                using (var wb = new WebClient())
                {
                    var data = new NameValueCollection();
                    data["secret"] = configuration["AppSettings:ReCaptchaSecretKey"];
                    data["response"] = captcha;

                    var url = configuration["AppSettings:ReCaptchaUrl"];
                    var response = wb.UploadValues(url, "POST", data);
                    dynamic googleResponse = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(response));
                    if (googleResponse.success == true)
                    {
                        this.feedbacks.Add(new Data.Models.Feedback()
                        {
                            Name = model.Name,
                            Email = model.Email,
                            Text = model.Comment,
                            IsNew = true
                        });

                        this.ViewBag.MailSend = true;
                    }
                }
            }

            var viewModel = new ContactsViewModel()
            {
                Email = configuration["AppSettings:ContactsEmail"],
                Phone = configuration["AppSettings:ContactsPhone"],
                SiteKey = configuration["AppSettings:ReCaptchaSiteKey"]
            };

            return this.View(viewModel);
        }

        public IActionResult Error()
        {
            this.AddTagsToViewBag(this.tags);
            this.ViewBag.PageDescription = "Грешка.";
            var exceptionHandler = this.HttpContext.Features.Get<IExceptionHandlerFeature>();
            var ipAddress = this.HttpContext.Connection.RemoteIpAddress;

            if (exceptionHandler != null)
            {
                this.logger.Log(LogLevel.Error, exceptionHandler.Error, "500", ipAddress);
            }

            return this.View();
        }

        [Route("Home/404")]
        public IActionResult PageNotFound()
        {
            this.AddTagsToViewBag(this.tags);
            this.ViewBag.PageDescription = "Несъществуваща страница.";
            var ipAddress = this.HttpContext.Connection.RemoteIpAddress;

            // this.logger.Log(LogLevel.Warn, this.HttpContext.Items["originalPath"]?.ToString() ?? string.Empty, "400", ipAddress);

            return this.View();
        }

        public IActionResult ImageHelp()
        {
            return this.View();
        }

        [Route("s")]
        public IActionResult Status()
        {
            var orders = this.orders.Get().Where(x => (x.ConfirmationStatus == ConfirmationStatus.Pending || x.ConfirmationStatus == ConfirmationStatus.Confirmed) && x.OrderStatus == OrderStatus.InProgress).Count();
            var feedbacks = this.feedbacks.Get().Where(x => x.IsNew).Count();

            return this.Content($"{orders}:{feedbacks}", "text/plain", Encoding.UTF8);
        }

        [Route("sitemap.xml")]
        public IActionResult SitemapXml()
        {
            var nodes = new List<SitemapNode>();

            nodes.Add(new SitemapNode() { Priority = 1, Frequency = SitemapFrequency.Weekly, Url = this.Url.RouteUrl("Default", new { action = "Index" }, this.Request.Scheme) });
            nodes.Add(new SitemapNode() { Priority = 0.1, Url = this.Url.RouteUrl("Default", new { action = "Contacts" }, this.Request.Scheme) });
            nodes.Add(new SitemapNode() { Priority = 0.3, Frequency = SitemapFrequency.Weekly, Url = this.Url.RouteUrl("Default", new { controller = "Gallery", action = "Index" }, this.Request.Scheme) });
            if (this.blog.GetPosts(x => x.Active).Count() > 0)
            {
                nodes.Add(new SitemapNode() { Priority = 0.6, Url = this.Url.RouteUrl("Default", new { controller = "Blog", action = "Index" }, this.Request.Scheme) });
            }

            var products = this.products.Get().Where(p => p.Active).OrderByDescending(p => p.Id).ToList();
            foreach (var product in products)
            {
                nodes.Add(new SitemapNode()
                {
                    Priority = 0.9,
                    Url = this.Url.RouteUrl("Product", new { acronym = product.Acronym }, this.Request.Scheme)
                });
            }

            var categories = this.categories.Get().OrderBy(c => c.Order).ToList();
            foreach (var category in categories)
            {
                nodes.Add(new SitemapNode()
                {
                    Priority = 0.3,
                    Frequency = SitemapFrequency.Monthly,
                    Url = this.Url.RouteUrl("GalleryCategory", new { acronym = category.Acronym }, this.Request.Scheme)
                });
            }

            var posts = this.blog.GetPosts(x => x.Active).ToList();
            foreach (var post in posts)
            {
                nodes.Add(new SitemapNode()
                {
                    Priority = 0.4,
                    Url = this.Url.RouteUrl("BlogPost", new { acronym = post.Acronym }, this.Request.Scheme)
                });
            }

            var xml = this.GetSitemapDocument(nodes);
            return this.Content(xml, "application/xml", Encoding.UTF8);
        }

        private string GetSitemapDocument(IEnumerable<SitemapNode> sitemapNodes)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xmlns + "urlset");

            foreach (SitemapNode sitemapNode in sitemapNodes)
            {
                XElement urlElement = new XElement(
                    xmlns + "url",
                    new XElement(xmlns + "loc", sitemapNode.Url),
                    sitemapNode.LastModified == null ? null : new XElement(
                        xmlns + "lastmod",
                        sitemapNode.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
                    sitemapNode.Frequency == null ? null : new XElement(
                        xmlns + "changefreq",
                        sitemapNode.Frequency.Value.ToString().ToLowerInvariant()),
                    sitemapNode.Priority == null ? null : new XElement(
                        xmlns + "priority",
                        sitemapNode.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));
                root.Add(urlElement);
            }

            XDocument document = new XDocument(root);
            return document.ToString();
        }
    }
}

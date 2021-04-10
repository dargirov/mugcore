using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MugStore.Data.Models;
using MugStore.Services.Common;
using MugStore.Services.Data;
using MugStore.Web.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MugStore.Web.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrdersService orders;
        private readonly IImagesService images;
        private readonly ICitiesService cities;
        private readonly IProductsService products;
        private readonly ICouriersService couriers;
        private readonly IConfiguration configuration;
        private readonly ILoggerService logger;
        private readonly ITagsService tags;
        private readonly IMailService mailService;

        public OrderController(IOrdersService orders, IImagesService images, ICitiesService cities, IProductsService products, ICouriersService couriers, IConfiguration configuration, ILoggerService logger, ITagsService tags, IMailService mailService)
        {
            this.orders = orders;
            this.images = images;
            this.cities = cities;
            this.products = products;
            this.couriers = couriers;
            this.configuration = configuration;
            this.logger = logger;
            this.tags = tags;
            this.mailService = mailService;
        }

        [HttpGet("/o/{acronym}")]
        public IActionResult Index(string acronym)
        {
            if (string.IsNullOrWhiteSpace(acronym) || acronym.Length != 10)
            {
                return RedirectToAction("Index", "Home");
            }

            var order = this.orders.Get(acronym);
            if (order == null || (DateTime.Now - order.CreatedOn).TotalDays > 60)
            {
                return RedirectToAction("Index", "Home");
            }

            var colorMugs = this.GetColorMugs(this.configuration).ToList();
            this.ViewBag.SingleMugPrice = decimal.Parse(configuration["AppSettings:SingleMugPrice"]);
            this.ViewBag.SingleMugMsrpPrice = decimal.Parse(configuration["AppSettings:SingleMugMsrpPrice"]);
            this.ViewBag.ShowRight = false;
            this.ViewBag.ColorMugs = colorMugs;
            this.ViewBag.ColorMugsEnabled = false;
            this.ViewBag.ColorMugName = colorMugs.FirstOrDefault(x => x.Color?.ToLowerInvariant() == order.Color?.ToLowerInvariant())?.Name ?? "Бял";
            this.AddTagsToViewBag(this.tags);
            this.ViewBag.PageDescription = $"Поръчка №{acronym}, преглед на поръчка";

            var viewModel = new IndexViewModel
            {
                Order = order,
                PageTitle = "Поръчка №" + acronym,
                TotalPrice = CalculateTotalPrice(order),
                DeliveryPrice = decimal.Parse(this.configuration["AppSettings:DeliveryPrice"])
            };

            if (order.ProductId.HasValue && order.ProductId > 0)
            {
                viewModel.PreviewImage = products.Get(order.ProductId.Value).Images.FirstOrDefault(x => x.Preview3d == true);
            }

            return View(viewModel);
        }

        public IActionResult Create([FromBody]CreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Json(new { status = "error" });
            }

            var city = this.cities.Get(model.DeliveryInfo.CityId);
            if (city == null)
            {
                return this.Json(new { status = "error", message = "Invalid city id" });
            }

            Courier courier = null;
            if (model.DeliveryInfo.CourierId.HasValue)
            {
                courier = this.couriers.Get(model.DeliveryInfo.CourierId.Value);
                if (courier == null)
                {
                    return this.Json(new { status = "error", message = "Invalid courier id" });
                }
            }

            var priceCustomer = decimal.Parse(this.configuration["AppSettings:SingleMugPrice"], CultureInfo.InvariantCulture);
            var priceSupplier = decimal.Parse(this.configuration["AppSettings:SungleMugPriceSupplier"], CultureInfo.InvariantCulture);
            var colorMug = this.GetColorMugs(this.configuration).FirstOrDefault(x => x.Color.ToLowerInvariant() == model.Color?.ToLowerInvariant());
            if (colorMug != null)
            {
                priceCustomer = colorMug.SingleMugPrice;
                priceSupplier = colorMug.SungleMugPriceSupplier;
            }

            var order = new Order()
            {
                Acronym = this.GenerateAcronym(),
                Quantity = model.Quantity,
                PriceCustomer = priceCustomer,
                PriceSupplier = priceSupplier,
                PriceDelivery = decimal.Parse(this.configuration["AppSettings:DeliveryPrice"], CultureInfo.InvariantCulture),
                PaymentMethod = model.PaymentMethod,
                DeliveryInfo = new DeliveryInfo()
                {
                    FullName = model.DeliveryInfo.FullName,
                    Address = model.DeliveryInfo.Address,
                    CityId = model.DeliveryInfo.CityId,
                    Comment = !string.IsNullOrWhiteSpace(model.DeliveryInfo.Comment) ? model.DeliveryInfo.Comment : null,
                    Phone = model.DeliveryInfo.Phone,
                    CourierId = model.DeliveryInfo.CourierId,
                    Email = !string.IsNullOrWhiteSpace(model.DeliveryInfo.Email) ? model.DeliveryInfo.Email : null,
                },
                ConfirmationStatus = ConfirmationStatus.Pending,
                OrderStatus = OrderStatus.InProgress,
                Color = model.Color
            };

            if (model.ProductAcronym != null)
            {
                var product = this.products.Get(model.ProductAcronym);
                if (product == null)
                {
                    return NotFound(model.ProductAcronym);
                }

                order.Product = product;
            }

            this.orders.Create(order);

            if (model.ProductAcronym == null)
            {
                foreach (var imageInfo in model.Images)
                {
                    var image = this.images.Get(imageInfo.Name);
                    image.OrderId = order.Id;
                    image.Rotation = imageInfo.Rotation;
                    image.Y = imageInfo.Y;
                }

                this.images.Save();
            }

            var result = new
            {
                status = "success",
                acronym = order.Acronym,
                paymentMethod = order.PaymentMethod,
                fullName = order.DeliveryInfo.FullName,
                address = order.DeliveryInfo.Address,
                phone = order.DeliveryInfo.Phone,
                comment = order.DeliveryInfo.Comment,
                email = order.DeliveryInfo.Email,
                city = city.Name,
                courier = courier?.Name,
                quantity = order.Quantity,
                price = CalculateTotalPrice(order),
                color = order.Color,
            };

            return this.Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(string acronym)
        {
            var order = this.orders.Get().FirstOrDefault(x => x.Acronym == acronym);
            if (order == null)
            {
                return BadRequest();
            }

            try
            {
                await AzureFunctionSendMailAsync(order);
                if (!string.IsNullOrWhiteSpace(order.DeliveryInfo.Email))
                {
                    await this.mailService.SendMailAsync(order);
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex, "500");
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult Feedback(string acronym, string message)
        {
            var order = this.orders.Get().FirstOrDefault(x => x.Acronym == acronym);
            if (order == null || string.IsNullOrWhiteSpace(message) || message.Length > 1000)
            {
                return BadRequest();
            }

            order.Feedback = message;
            this.orders.Save();
            return this.Ok();
        }

        private string GenerateAcronym()
        {
            var date = DateTime.UtcNow;
            var rand = new Random();

            var acronym = string.Format("{0}{1}", date.ToString("yyMMdd"), rand.Next(1000, 9999));
            var order = this.orders.Get(acronym);

            while (order != null)
            {
                acronym = string.Format("{0}{1}", date.ToString("yyMMdd"), rand.Next(1000, 9999));
                order = this.orders.Get(acronym);
            }

            return acronym;
        }

        private decimal CalculateTotalPrice(Order order)
        {
            return order.Quantity * order.PriceCustomer + order.PriceDelivery;
        }

        private async Task AzureFunctionSendMailAsync(Order order)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-functions-key", this.configuration["AppSettings:AzureFunctionKey"]);

            var values = new Dictionary<string, string>
            {
                ["orderId"] = order.Acronym,
                ["courier"] = order.DeliveryInfo.Courier?.Name,
                ["quantity"] = order.Quantity.ToString(),
            };

            var content = new FormUrlEncodedContent(values);
            await client.PostAsync(this.configuration["AppSettings:AzureFunctionUrl"], content);
        }
    }
}

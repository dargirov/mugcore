using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MugStore.Data.Models;
using MugStore.Services.Common;
using MugStore.Services.Data;
using MugStore.Web.ViewModels.Order;
using System;
using System.Collections.Generic;
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

        public OrderController(IOrdersService orders, IImagesService images, ICitiesService cities, IProductsService products, ICouriersService couriers, IConfiguration configuration, ILoggerService logger, ITagsService tags)
        {
            this.orders = orders;
            this.images = images;
            this.cities = cities;
            this.products = products;
            this.couriers = couriers;
            this.configuration = configuration;
            this.logger = logger;
            this.tags = tags;
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

            this.ViewBag.ShowRight = false;
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

            var order = new Order()
            {
                Acronym = this.GenerateAcronym(),
                Quantity = model.Quantity,
                PriceCustomer = decimal.Parse(this.configuration["AppSettings:SingleMugPrice"]),
                PriceSupplier = decimal.Parse(this.configuration["AppSettings:SungleMugPriceSupplier"]),
                PriceDelivery = decimal.Parse(this.configuration["AppSettings:DeliveryPrice"]),
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
                OrderStatus = OrderStatus.InProgress
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

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-functions-key", this.configuration["AppSettings:AzureFunctionKey"]);

            var values = new Dictionary<string, string>
            {
                ["orderId"] = order.Acronym,
                ["courier"] = order.DeliveryInfo.Courier?.Name,
                ["quantity"] = order.Quantity.ToString(),
            };

            var content = new FormUrlEncodedContent(values);

            try
            {
                await client.PostAsync(this.configuration["AppSettings:AzureFunctionUrl"], content);
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e.Message, "500");
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
            return (order.Quantity * decimal.Parse(this.configuration["AppSettings:SingleMugPrice"])) + decimal.Parse(this.configuration["AppSettings:DeliveryPrice"]);
        }
    }
}

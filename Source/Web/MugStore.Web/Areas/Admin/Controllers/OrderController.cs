using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MugStore.Data.Models;
using MugStore.Services.Data;
using MugStore.Web.Areas.Admin.ViewModels.Order;
using MugStore.Web.Controllers;
using System.Linq;

namespace MugStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "LoggedIn")]
    public class OrderController : BaseController
    {
        private readonly IOrdersService orders;
        private readonly IImagesService images;

        public OrderController(IOrdersService orders, IImagesService images)
        {
            this.orders = orders;
            this.images = images;
        }

        public IActionResult Index()
        {
            var orders = this.orders.Get().OrderByDescending(o => o.Id).ToList();
            var viewModel = new IndexViewModel()
            {
                Orders = orders
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetStatus(int orderId, ConfirmationStatus confirmationStatus, OrderStatus orderStatus, string type)
        {
            var order = this.orders.Get(orderId);
            if (order == null)
            {
                return this.Json(new { success = false });
            }

            switch (type)
            {
                case "confirmation":
                    order.ConfirmationStatus = confirmationStatus;
                    break;
                case "order":
                    order.OrderStatus = orderStatus;
                    break;
            }

            this.orders.Save();

            return this.Json(new { success = true });
        }

        public IActionResult Preview(int id)
        {
            var order = this.orders.Get(id);
            if (order == null)
            {
                return BadRequest(id.ToString());
            }

            var viewModel = this.Mapper.Map<PreviewViewModel>(order);

            return this.View(viewModel);
        }

        public IActionResult UploadedImages()
        {
            var images = this.images.Get().OrderByDescending(i => i.Id).ToList();
            var viewModel = new UploadedImagesViewModel()
            {
                Images = images
            };

            return this.View(viewModel);
        }
    }
}

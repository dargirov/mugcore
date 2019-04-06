using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MugStore.Data.Models;
using MugStore.Services.Data;
using MugStore.Web.Areas.Admin.ViewModels.Order;
using MugStore.Web.Controllers;
using System;
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

        public IActionResult Index(IndexInputModel model)
        {
            var orders = this.orders.Get();

            if (!model.All)
            {
                orders = orders.Where(x =>
                    (x.ConfirmationStatus == ConfirmationStatus.Confirmed || x.ConfirmationStatus == ConfirmationStatus.Pending)
                    && (x.OrderStatus == OrderStatus.InProgress || x.OrderStatus == OrderStatus.Sent || x.OrderStatus == OrderStatus.Finalized));
            }

            orders = orders.OrderByDescending(o => o.Id);

            var viewModel = new IndexViewModel()
            {
                Orders = orders.ToList(),
                All = model.All
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

        public IActionResult UploadedImages(int page = 1)
        {
            const int pageSize = 100;

            var images = this.images
                .Get()
                .OrderByDescending(i => i.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var imagesCount = this.images.Get().Count();

            var viewModel = new UploadedImagesViewModel()
            {
                Images = images,
                ImagesCount = imagesCount,
                Pages = (int)Math.Ceiling((decimal)imagesCount / pageSize),
                CurrentPage = page
            };

            return this.View(viewModel);
        }
    }
}

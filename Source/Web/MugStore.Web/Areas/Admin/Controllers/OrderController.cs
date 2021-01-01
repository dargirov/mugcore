using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MugStore.Data.Models;
using MugStore.Services.Data;
using MugStore.Web.Areas.Admin.ViewModels.Order;
using MugStore.Web.Controllers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MugStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "LoggedIn")]
    public class OrderController : BaseController
    {
        private int PageSize => 100;
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
            var page = model.Page > 0 ? model.Page : 1;
            var ordersCount = 0;

            if (model.All)
            {
                orders = orders.OrderByDescending(o => o.Id);
                ordersCount = this.orders.Get().Count();
            }
            else
            {
                Expression<Func<Order, bool>> statusPredicate = x =>
                    (x.ConfirmationStatus == ConfirmationStatus.Confirmed || x.ConfirmationStatus == ConfirmationStatus.Pending)
                    && (x.OrderStatus == OrderStatus.InProgress || x.OrderStatus == OrderStatus.Sent || x.OrderStatus == OrderStatus.Finalized);
                orders = orders
                    .Where(statusPredicate)
                    .OrderByDescending(o => o.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize);
                ordersCount = this.orders.Get().Where(statusPredicate).Count();
            }

            var viewModel = new IndexViewModel()
            {
                Orders = orders.ToList(),
                All = model.All,
                Pages = (int)Math.Ceiling((decimal)ordersCount / PageSize),
                CurrentPage = page,
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
            var images = this.images
                .Get()
                .OrderByDescending(i => i.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            var imagesCount = this.images.Get().Count();

            var viewModel = new UploadedImagesViewModel()
            {
                Images = images,
                Pages = (int)Math.Ceiling((decimal)imagesCount / PageSize),
                CurrentPage = page
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Note(NoteInputModel model)
        {
            var order = this.orders.Get(model.OrderId);
            order.Note = string.IsNullOrWhiteSpace(model.Note)
                ? null
                : model.Note;

            this.orders.Save();

            return RedirectToAction(nameof(OrderController.Index));
        }
    }
}

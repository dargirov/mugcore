﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MugStore.Data.Models;
using MugStore.Services.Common;
using MugStore.Services.Data;
using MugStore.Web.Areas.Admin.ViewModels.Home;
using MugStore.Web.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace MugStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : BaseController
    {
        private readonly IOrdersService orders;
        private readonly IBulletinsService bulletins;
        private readonly IImagesService images;
        private readonly IFeedbacksService feedbacks;
        private readonly ILoggerService logger;
        private readonly IConfiguration configuration;

        public HomeController(IOrdersService orders, IBulletinsService bulletin, IImagesService images, IFeedbacksService feedbacks, ILoggerService logger, IConfiguration configuration)
        {
            this.orders = orders;
            this.bulletins = bulletin;
            this.images = images;
            this.feedbacks = feedbacks;
            this.logger = logger;
            this.configuration = configuration;
        }

        [Authorize(Policy = "LoggedIn")]
        public IActionResult Index()
        {
            var orders = this.orders.Get().Where(o => o.ConfirmationStatus != ConfirmationStatus.Denied).ToList();
            var bulletins = this.bulletins.Get().OrderByDescending(b => b.Id).ToList();
            var feedbacks = this.feedbacks.Get().OrderByDescending(x => x.IsNew).ThenByDescending(x => x.Id).ToList();
            var images = this.images.Get().OrderByDescending(i => i.Id).ToList();
            var priceChartOrders = new List<Order>();

            foreach (var order in this.orders.Get().Where(o => o.OrderStatus == OrderStatus.Finalized))
            {
                if (!priceChartOrders.Any(x => x.CreatedOn.ToShortDateString() == order.CreatedOn.ToShortDateString()))
                {
                    priceChartOrders.Add(order);
                }
            }

            var viewModel = new IndexViewModel()
            {
                Orders = orders,
                Bulletin = bulletins,
                Images = images,
                PriceChartOrders = priceChartOrders,
                Feedbacks = feedbacks,
                LogMessages = this.logger.GetLogMessages()
            };

            return this.View(viewModel);
        }

        [Authorize(Policy = "LoggedIn")]
        public IActionResult Logout()
        {
            this.HttpContext.Session.Remove("logged_in");
            this.HttpContext.Session.Clear();
            return this.RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return this.PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Index");
            }

            var email = this.configuration["AppSettings:AdminLoginEmail"];
            var pass = this.configuration["AppSettings:AdminLoginPassword"];

            if (model.Email.CompareTo(email) == 0 && model.Password.CompareTo(pass) == 0)
            {
                this.HttpContext.Session.SetString("logged_in", "true");
            }

            return this.RedirectToAction("Index");
        }

        [Authorize(Policy = "LoggedIn")]
        public IActionResult Bulletin()
        {
            var bulletins = this.bulletins.Get().OrderByDescending(b => b.Id).ToList();
            var viewModel = new BulletinViewModel()
            {
                Bulletins = bulletins
            };

            return this.View(viewModel);
        }

        [Authorize(Policy = "LoggedIn")]
        [HttpPost]
        public IActionResult FeedbackStatus(int id)
        {
            var feedback = this.feedbacks.Get().Where(x => x.Id == id).FirstOrDefault();
            if (feedback == null)
            {
                return NotFound(id.ToString());
            }

            feedback.IsNew = false;
            this.feedbacks.Save();
            return null;
        }

        [Authorize(Policy = "LoggedIn")]
        [HttpPost]
        public IActionResult FeedbackDelete(int id)
        {
            var feedback = this.feedbacks.Get().Where(x => x.Id == id).FirstOrDefault();
            if (feedback == null)
            {
                return NotFound(id.ToString());
            }

            this.feedbacks.Delete(feedback);
            return null;
        }
    }
}

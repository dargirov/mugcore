using Microsoft.AspNetCore.Mvc;
using MugStore.Data.Models;
using MugStore.Services.Data;
using MugStore.Web.ViewModels.Bulletin;

namespace MugStore.Web.Controllers
{
    public class BulletinController : BaseController
    {
        private readonly IBulletinsService bulletins;

        public BulletinController(IBulletinsService bulletins)
        {
            this.bulletins = bulletins;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(BulletinInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Json(new { success = false, message = "Invalid email" });
            }

            var bulletin = this.bulletins.Get(model.Email);
            if (bulletin != null)
            {
                return this.Json(new { success = false, message = "Email exists" });
            }

            this.bulletins.Add(new Bulletin()
            {
                Email = model.Email
            });

            return this.Json(new { success = true });
        }
    }
}

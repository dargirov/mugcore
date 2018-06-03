using MugStore.Data.Models;
using System.Collections.Generic;

namespace MugStore.Web.Areas.Admin.ViewModels.Home
{
    public class BulletinViewModel
    {
        public IEnumerable<Bulletin> Bulletins { get; set; }
    }
}

using System.Collections.Generic;

namespace MugStore.Web.Areas.Admin.ViewModels.Order
{
    public class IndexViewModel
    {
        public IEnumerable<Data.Models.Order> Orders { get; set; }
        public bool All { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}

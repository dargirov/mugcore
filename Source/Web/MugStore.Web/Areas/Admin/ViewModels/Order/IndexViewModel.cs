using System.Collections.Generic;

namespace MugStore.Web.Areas.Admin.ViewModels.Order
{
    public class IndexViewModel
    {
        public IEnumerable<Data.Models.Order> Orders { get; set; }
    }
}

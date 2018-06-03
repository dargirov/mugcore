using System.Collections.Generic;

namespace MugStore.Web.Areas.Admin.ViewModels.Product
{
    public class IndexViewModel
    {
        public IEnumerable<Data.Models.Product> Products { get; set; }
    }
}

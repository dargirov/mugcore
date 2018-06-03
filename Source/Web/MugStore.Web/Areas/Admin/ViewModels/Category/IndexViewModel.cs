using System.Collections.Generic;

namespace MugStore.Web.Areas.Admin.ViewModels.Category
{
    public class IndexViewModel
    {
        public IEnumerable<Data.Models.Category> Categories { get; set; }
    }
}

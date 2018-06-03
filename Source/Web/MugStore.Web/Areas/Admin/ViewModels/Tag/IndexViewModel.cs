using MugStore.Data.Models;
using System.Collections.Generic;

namespace MugStore.Web.Areas.Admin.ViewModels.Tag
{
    public class IndexViewModel
    {
        public IEnumerable<ProductTag> ProductTags { get; set; }

        public IEnumerable<PostTag> PostTags { get; set; }
    }
}

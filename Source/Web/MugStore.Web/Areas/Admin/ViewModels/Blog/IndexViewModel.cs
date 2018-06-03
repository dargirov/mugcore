using System.Collections.Generic;
using MugStore.Data.Models;

namespace MugStore.Web.Areas.Admin.ViewModels.Blog
{
    public class IndexViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
    }
}
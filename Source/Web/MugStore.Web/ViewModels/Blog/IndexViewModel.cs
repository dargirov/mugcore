using MugStore.Data.Models;
using System.Collections.Generic;

namespace MugStore.Web.ViewModels.Blog
{
    public class IndexViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
    }
}
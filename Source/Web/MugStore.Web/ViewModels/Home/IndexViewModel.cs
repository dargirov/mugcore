using System.Collections.Generic;

namespace MugStore.Web.ViewModels.Home
{
    public class IndexViewModel
    {
        public IEnumerable<Data.Models.Product> Products { get; set; }

        public string MugInfoType { get; set; }

        public IEnumerable<Data.Models.Post> BlogPosts { get; set; }
    }
}

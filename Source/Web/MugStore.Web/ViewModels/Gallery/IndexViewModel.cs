using MugStore.Data.Models;
using System.Collections.Generic;

namespace MugStore.Web.ViewModels.Gallery
{
    public class IndexViewModel
    {
        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<Data.Models.Product> Products { get; set; }

        public string CategoryName { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public string BaseUrl { get; set; }

        public string Acronym { get; set; }

        public string Type { get; set; }
    }
}

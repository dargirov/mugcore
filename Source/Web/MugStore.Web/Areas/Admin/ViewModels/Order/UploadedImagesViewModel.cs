using MugStore.Data.Models;
using System.Collections.Generic;

namespace MugStore.Web.Areas.Admin.ViewModels.Order
{
    public class UploadedImagesViewModel
    {
        public IEnumerable<Image> Images { get; set; }
        public int ImagesCount { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}

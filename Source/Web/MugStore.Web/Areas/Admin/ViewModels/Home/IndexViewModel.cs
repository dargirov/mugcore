using MugStore.Data.Models;
using System;
using System.Collections.Generic;

namespace MugStore.Web.Areas.Admin.ViewModels.Home
{
    public class IndexViewModel
    {
        public IEnumerable<Data.Models.Order> Orders { get; set; }

        public IEnumerable<Bulletin> Bulletin { get; set; }

        public IEnumerable<Feedback> Feedbacks { get; set; }

        public int ImagesCount { get; set; }

        public IEnumerable<Data.Models.Order> PriceChartOrders { get; set; }

        public IEnumerable<Log> LogErrorMessages { get; set; }

        public IEnumerable<Log> LogNotFoundMessages { get; set; }

        public DateTime StatsFrom { get; set; }

        public DateTime StatsTo { get; set; }
    }
}

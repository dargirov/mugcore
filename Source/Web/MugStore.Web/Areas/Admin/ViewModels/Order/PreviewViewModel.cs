using MugStore.Data.Models;
using MugStore.Web.Infrastructure.Mapping;
using System.Collections.Generic;

namespace MugStore.Web.Areas.Admin.ViewModels.Order
{
    public class PreviewViewModel : IMapFrom<Data.Models.Order>
    {
        public string Acronym { get; set; }

        public IEnumerable<Image> Images { get; set; }

        public Data.Models.Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal PriceCustomer { get; set; }

        public decimal PriceDelivery { get; set; }
    }
}

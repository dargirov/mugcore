namespace MugStore.Web.ViewModels.Order
{
    public class IndexViewModel
    {
        public Data.Models.Order Order { get; set; }

        public string PageTitle { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal DeliveryPrice { get; set; }
    }
}

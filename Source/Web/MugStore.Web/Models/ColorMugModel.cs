namespace MugStore.Web.Models
{
    public class ColorMugModel
    {
        public string Color { get; set; }
        public string Name { get; set; }
        public decimal SingleMugPrice { get; set; }
        public decimal SingleMugMsrpPrice { get; set; }
        public decimal SungleMugPriceSupplier { get; set; }
        public bool Enabled { get; set; }
    }
}

namespace MugStore.Data.Models
{
    public class ProductTagProduct
    {
        public int ProductTagId { get; set; }
        public ProductTag ProductTag { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}

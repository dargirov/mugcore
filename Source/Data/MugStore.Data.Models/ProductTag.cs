using MugStore.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MugStore.Data.Models
{
    public class ProductTag : BaseModel<int>
    {
        public ProductTag()
        {
            this.Products = new HashSet<ProductTagProduct>();
        }

        [Required]
        [MaxLength(GlobalConstants.ProductTagMaxlength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(GlobalConstants.ProductTagAcronymMaxlength)]
        public string Acronym { get; set; }

        [Required]
        public bool Active { get; set; }

        public virtual ICollection<ProductTagProduct> Products { get; set; }
    }
}

using MugStore.Common;
using System.ComponentModel.DataAnnotations;

namespace MugStore.Data.Models
{
    public class ProductImage : BaseModel<int>
    {
        [Required]
        [MaxLength(GlobalConstants.MaxImageNameLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxImageOriginalNameLength)]
        public string OriginalName { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxImageContentTypeLength)]
        public string ContentType { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxImagePathLength)]
        public string Path { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public bool Preview3d { get; set; }

        [Required]
        public bool Thumb { get; set; }
    }
}

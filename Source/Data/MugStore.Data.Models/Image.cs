using MugStore.Common;
using System.ComponentModel.DataAnnotations;

namespace MugStore.Data.Models
{
    public class Image : BaseModel<int>
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
        public int Width { get; set; }

        [Required]
        public int Height { get; set; }

        [Required]
        public int Dpi { get; set; }

        public double Rotation { get; set; }

        public double Y { get; set; }

        public int? OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}

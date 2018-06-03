using MugStore.Common;
using System.ComponentModel.DataAnnotations;

namespace MugStore.Data.Models
{
    public class Category : BaseModel<int>
    {
        [Required]
        [MaxLength(GlobalConstants.MaxCategoryNameLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxCategoryAcronymLength)]
        public string Acronym { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}

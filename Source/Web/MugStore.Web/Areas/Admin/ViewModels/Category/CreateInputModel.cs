using MugStore.Common;
using System.ComponentModel.DataAnnotations;

namespace MugStore.Web.Areas.Admin.ViewModels.Category
{
    public class CreateInputModel
    {
        [Required]
        [MaxLength(GlobalConstants.MaxCategoryNameLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxCategoryAcronymLength)]
        public string Acronym { get; set; }

        [Required]
        public int Order { get; set; }
    }
}

using MugStore.Common;
using System.ComponentModel.DataAnnotations;

namespace MugStore.Data.Models
{
    public class Courier : BaseModel<int>
    {
        [Required]
        [MaxLength(GlobalConstants.MaxCourierNameLength)]
        public string Name { get; set; }

        [Required]
        public bool Active { get; set; }

        public string Info { get; set; }
    }
}

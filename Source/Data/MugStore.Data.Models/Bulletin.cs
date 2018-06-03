using MugStore.Common;
using System.ComponentModel.DataAnnotations;

namespace MugStore.Data.Models
{
    public class Bulletin : BaseModel<int>
    {
        [Required]
        [MaxLength(GlobalConstants.EmailMaxLength)]
        public string Email { get; set; }

        public bool Verified { get; set; }
    }
}

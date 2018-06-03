using MugStore.Common;
using System.ComponentModel.DataAnnotations;

namespace MugStore.Data.Models
{

    public class Feedback : BaseModel<int>
    {
        [Required]
        [MaxLength(GlobalConstants.MaxFeedbackNameLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(GlobalConstants.EmailMaxLength)]
        public string Email { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public bool IsNew { get; set; }
    }
}

using MugStore.Common;
using System.ComponentModel.DataAnnotations;

namespace MugStore.Web.ViewModels.Bulletin
{
    public class BulletinInputModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(GlobalConstants.EmailMaxLength)]
        public string Email { get; set; }
    }
}

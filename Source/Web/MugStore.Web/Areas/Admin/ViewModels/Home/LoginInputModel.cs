using System.ComponentModel.DataAnnotations;

namespace MugStore.Web.Areas.Admin.ViewModels.Home
{
    public class LoginInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}

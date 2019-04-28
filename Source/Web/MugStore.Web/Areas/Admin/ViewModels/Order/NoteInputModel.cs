using System.ComponentModel.DataAnnotations;

namespace MugStore.Web.Areas.Admin.ViewModels.Order
{
    public class NoteInputModel
    {
        [Required]
        public int OrderId { get; set; }

        public string Note { get; set; }
    }
}

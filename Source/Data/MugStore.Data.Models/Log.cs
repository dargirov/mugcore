using System.ComponentModel.DataAnnotations;

namespace MugStore.Data.Models
{
    public class Log : BaseModel<int>
    {
        [Required]
        public LogLevel Level { get; set; }

        [MaxLength(100)]
        public string Code { get; set; }

        [Required]
        public string Content { get; set; }

        [MaxLength(255)]
        public string IpAddress { get; set; }
    }
}

using MugStore.Common;
using System.ComponentModel.DataAnnotations;

namespace MugStore.Data.Models
{
    public class City : BaseModel<int>
    {
        [Required]
        [MaxLength(GlobalConstants.MaxCityNameLength)]
        public string Name { get; set; }

        [Required]
        public int PostCode { get; set; }

        [Required]
        public CityType Type { get; set; }

        [Required]
        public bool Highlight { get; set; }
    }
}

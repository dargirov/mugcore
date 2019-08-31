﻿using MugStore.Common;
using System.ComponentModel.DataAnnotations;

namespace MugStore.Data.Models
{
    public class DeliveryInfo : BaseModel<int>
    {
        [Required]
        [MaxLength(GlobalConstants.MaxDeliveryInfoFullNameLength)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxDeliveryInfoPhoneLength)]
        public string Phone { get; set; }

        [Required]
        public int CityId { get; set; }

        public virtual City City { get; set; }

        [Required]
        public string Address { get; set; }

        public string Comment { get; set; }

        public int? CourierId { get; set; }

        public virtual Courier Courier { get; set; }

        [MaxLength(GlobalConstants.MaxDeliveryInfoEmail)]
        public string Email { get; set; }
    }
}

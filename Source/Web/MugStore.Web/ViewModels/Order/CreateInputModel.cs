﻿using MugStore.Common;
using MugStore.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MugStore.Web.ViewModels.Order
{
    public class CreateInputModel
    {
        [Required]
        [Range(1, GlobalConstants.MaxOrderQuantity)]
        public int Quantity { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public DeliveryInfo DeliveryInfo { get; set; }

        public List<ImageInputModel> Images { get; set; }

        public string ProductAcronym { get; set; }

        public string Color { get; set; }
    }
}

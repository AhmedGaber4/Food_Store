﻿using Store.Service.Services.BasketService.Dots;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.OrderService.Dto
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }
        public string BuyerEmail { get; set; }
        public AddressDto ShippingAddress { get; set; }

    }
}
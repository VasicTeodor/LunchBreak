﻿using System;
using System.Collections.Generic;

namespace LunchBreak.Shared.Models
{
    public class LunchDto
    {
        public string Id { get; set; }
        public string Restaurant { get; set; }
        public string RestaurantId { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string LinkToMenu { get; set; }
        public bool Approved { get; set; }
        public decimal TotalPrice { get; set; }
        public string IsPublic { get; set; }
        public string FreeDelivery { get; set; }
        public string TeamId { get; set; }
        public decimal DeliveryPrice { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
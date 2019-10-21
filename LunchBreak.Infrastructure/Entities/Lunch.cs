using System;
using System.Collections.Generic;

namespace LunchBreak.Infrastructure.Entities
{
    public class Lunch : BaseEntity
    {
        public string Restaurant { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string RestaurantId { get; set; }
        public bool Approved { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string LinkToMenu { get; set; }
        public decimal TotalPrice { get; set; }
        public string IsPublic { get; set; }
        public string FreeDelivery { get; set; }
        public decimal DeliveryPrice { get; set; }
        public string TeamId { get; set; }
        public string CreatedBy { get; set; }
        public List<Order> Orders { get; set; }
    }
}
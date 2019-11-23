using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LunchBreak.Shared.Models
{
    public class LunchDto
    {
        public string Id { get; set; }
        [StringLength(50, ErrorMessage = "Restaurant name too long (50 character limit).")]
        public string Restaurant { get; set; }
        [Required]
        public string RestaurantId { get; set; }
        public string CreatedBy { get; set; }
        [StringLength(150, ErrorMessage = "Description too long (150 character limit).")]
        public string Description { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Lunch name too long (50 character limit).")]
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string LinkToMenu { get; set; }
        public bool Approved { get; set; }
        public decimal TotalPrice { get; set; }
        [Required]
        public string IsPublic { get; set; }
        [Required]
        public string FreeDelivery { get; set; }
        public string TeamId { get; set; }
        public decimal DeliveryPrice { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
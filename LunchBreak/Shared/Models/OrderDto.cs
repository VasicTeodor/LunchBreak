using System.ComponentModel.DataAnnotations;

namespace LunchBreak.Shared.Models
{
    public class OrderDto
    {
        public string Id { get; set; }
        [Required]
        [StringLength(95, ErrorMessage = "Order text too long (95 character limit).")]
        public string OrderText { get; set; }
        public string Comment { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
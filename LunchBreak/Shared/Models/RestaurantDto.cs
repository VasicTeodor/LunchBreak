using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LunchBreak.Shared.Models
{
    public class RestaurantDto
    {
        public string Id { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Restaurant name too long (20 character limit).")]
        public string Name { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Restaurant description too long (50 character limit).")]
        public string Description { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Address too long (50 character limit).")]
        public string Address { get; set; }
        public bool Approved { get; set; }
        [Required]
        [StringLength(9, ErrorMessage = "Phone number too long (9 character limit).")]
        public string Phone { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Restaurant type too long (20 character limit).")]
        public string Type { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public int Grade { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}
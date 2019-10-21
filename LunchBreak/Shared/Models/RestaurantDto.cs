using System.Collections.Generic;

namespace LunchBreak.Shared.Models
{
    public class RestaurantDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Approved { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public int Grade { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}
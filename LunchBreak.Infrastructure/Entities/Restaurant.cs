using System.Collections.Generic;

namespace LunchBreak.Infrastructure.Entities
{
    public class Restaurant : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public bool Approved { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public int Grade { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
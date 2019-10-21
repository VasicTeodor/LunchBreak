using System;

namespace LunchBreak.Infrastructure.Entities
{
    public class Comment : BaseEntity
    {
        public string Text { get; set; }
        public int Grade { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool Approved { get; set; }
        public DateTime Date { get; set; }
    }
}
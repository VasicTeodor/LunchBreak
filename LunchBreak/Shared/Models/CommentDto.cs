using System;

namespace LunchBreak.Shared.Models
{
    public class CommentDto
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public int Grade { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool Apporved { get; set; }
        public DateTime Date { get; set; }
    }
}
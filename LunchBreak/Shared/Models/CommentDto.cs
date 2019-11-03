using System;
using System.ComponentModel.DataAnnotations;

namespace LunchBreak.Shared.Models
{
    public class CommentDto
    {
        public string Id { get; set; }
        [Required]
        [StringLength(95, ErrorMessage = "Comment text too long (95 character limit).")]
        public string Text { get; set; }
        [Required]
        public int Grade { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool Apporved { get; set; }
        public DateTime Date { get; set; }
    }
}
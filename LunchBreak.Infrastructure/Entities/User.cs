﻿namespace LunchBreak.Infrastructure.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string TeamId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public bool Approved { get; set; }
        public Image ProfilePicture { get; set; }
        public Image DocumentPicture { get; set; }
    }
}
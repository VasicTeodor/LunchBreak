using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LunchBreak.Shared
{
    public class UserRegisterDTO
    {
        [Required]
        [StringLength(16, ErrorMessage = "Identifier too long (16 character limit).")]
        public string Username { get; set; }
        [Required]
        [StringLength(25, ErrorMessage = "Identifier too long (25 character limit).")]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Please enter valid email address.")]
        public string Email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password must be 8 characters long.")]
        public string Password { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password must be 8 characters long.")]
        [Compare("Password", ErrorMessage = "Passwords must match.")]
        public string ConfirmPassword { get; set; }
        public string Id { get; set; }
        public string TeamId { get; set; }
    }
}

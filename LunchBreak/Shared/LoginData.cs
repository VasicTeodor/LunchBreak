using System.ComponentModel.DataAnnotations;

namespace LunchBreak.Shared
{
    public class LoginData
    {
        [Required]
        [StringLength(16, ErrorMessage = "Username too long (16 character limit).")]
        public string Username { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password must be 8 characters long.")]
        public string Password { get; set; }
    }
}
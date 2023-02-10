using System.ComponentModel.DataAnnotations;

namespace PortalBlazor.Shared.Models
{
    public class UserInfo
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
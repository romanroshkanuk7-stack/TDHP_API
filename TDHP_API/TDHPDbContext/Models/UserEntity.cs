using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TDHP_API.TDHPDbContext.Models
{
    public class UserEntity : IdentityUser
    {
        [Required]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime DateOfCreate { get; set; } = DateTime.UtcNow;
    }
}

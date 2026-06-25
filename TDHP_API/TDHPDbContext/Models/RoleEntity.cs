using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TDHP_API.TDHPDbContext.Models
{
    public class RoleEntity : IdentityRole
    {
        [Required]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime DateOfCreate { get; set; } = DateTime.UtcNow;
    }
}

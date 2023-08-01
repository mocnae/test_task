using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public bool IsBlocked { get; set; } = false;
    }
}
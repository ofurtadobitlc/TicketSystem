using Microsoft.AspNetCore.Identity;

namespace TicketSystem.Web.Models.Account
{
    public class AppUserRole: IdentityUserRole<string>
    {
        public virtual AppUser? User { get; set; }
        public virtual AppRole? Role { get; set; }
    }
}

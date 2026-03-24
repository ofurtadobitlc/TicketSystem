using Microsoft.AspNetCore.Identity;

namespace TicketSystem.Web.Models.Account
{
    public class AppUser: IdentityUser
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; } = [];
    }
}

using Microsoft.AspNetCore.Identity;

namespace TicketSystem.Web.Models.Account
{
    public class AppRole: IdentityRole
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; } = [];
    }
}

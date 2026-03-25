using Microsoft.AspNetCore.Identity;
using TicketSystem.Web.Models.Workflow;

namespace TicketSystem.Web.Models.Account
{
    public class AppRole: IdentityRole
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; } = [];
        public virtual ICollection<WorkflowStatusTransition> StatusTransitions { get; set; } = [];
    }
}

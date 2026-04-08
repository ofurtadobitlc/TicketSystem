using System.ComponentModel.DataAnnotations;
using TicketSystem.Web.Models.Account;

namespace TicketSystem.Web.Models.ProjectManagement
{
    public class ProjectMember
    {
        [Display(Name = "Project")]
        public int ProjectId { get; set; }
        public ProjectModel? Project { get; set; }

        [Display(Name = "Member")]
        public string MemberId { get; set; }
        public AppUser? Member { get; set; }
        public required string RoleInProject { get; set; }
    }
}

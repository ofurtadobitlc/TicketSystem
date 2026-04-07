using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Web.Models.Account;
using TicketSystem.Web.Models.Communication;
using TicketSystem.Web.Models.Project;
using TicketSystem.Web.Models.Ticket;
using TicketSystem.Web.Models.Workflow;

namespace TicketSystem.Web.Models
{
    public class AppDbContext: IdentityDbContext<AppUser, AppRole, string,
                                                IdentityUserClaim<string>, AppUserRole, 
                                                IdentityUserLogin<string>, 
                                                IdentityRoleClaim<string>,
                                                IdentityUserToken<string>>
    {


        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<TicketModel> Tickets { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketDependency> TicketDependencies { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }
        public DbSet<WorkflowModel> Workflows { get; set; }
        public DbSet<WorkflowStatus> WorkflowStatuses { get; set; }
        public DbSet<WorkflowStatus> WorkflowStatusTransitions { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            base.OnModelCreating(modelBuilder);

            // AppUser
            modelBuilder.Entity<AppUser>()
                .HasMany(au => au.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<AppRole>()
                .HasMany(ar => ar.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();


            // Ticket
            modelBuilder.Entity<TicketModel>()
                .HasOne(tm => tm.Project)
                .WithMany(p => p.Tickets)
                .HasForeignKey(tm => tm.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketModel>()
                .HasOne(tm => tm.CreatedBy)
                .WithMany(p => p.TicketsCreated)
                .HasForeignKey(tm => tm.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketModel>()
                .HasOne(tm => tm.Assignee)
                .WithMany(p => p.TicketsAssignedToUser)
                .HasForeignKey(tm => tm.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketModel>()
                .HasOne(tm => tm.ClosedBy)
                .WithMany(p => p.TicketsClosed)
                .HasForeignKey(tm => tm.ClosedById)
                .OnDelete(DeleteBehavior.Restrict);


            // TicketDependency
            // 1. Chave Composta
            modelBuilder.Entity<TicketDependency>()
                .HasKey(td => new { td.BlockedTicketId, td.BlockingTicketId });

            modelBuilder.Entity<TicketDependency>()
                .HasOne(td => td.BlockedTicket)
                .WithMany(t => t.BlockedByTickets)
                .HasForeignKey(td => td.BlockedTicketId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketDependency>()
                .HasOne(td => td.BlockingTicket)
                .WithMany(t => t.BlockingTickets) 
                .HasForeignKey(td => td.BlockingTicketId)
                .OnDelete(DeleteBehavior.Restrict);


            // Ticket Attachment
            modelBuilder.Entity<TicketAttachment>()
                .HasOne(ta => ta.Ticket)
                .WithMany(t => t.Attachments)
                .HasForeignKey(ta => ta.TicketId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketAttachment>()
                .HasOne(ta => ta.UploadedBy)
                .WithMany(t => t.FilesAttached)
                .HasForeignKey(ta => ta.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);


            // Workflow Status

            modelBuilder.Entity<WorkflowStatus>()
                .HasOne(ws => ws.Workflow)
                .WithMany(w => w.Statuses)
                .HasForeignKey(ws => ws.WorkflowId)
                .OnDelete(DeleteBehavior.Restrict);


            // Status Transition

            modelBuilder.Entity<WorkflowStatusTransition>()
                .HasOne(td => td.FromStatus)
                .WithMany(t => t.NextStatuses)
                .HasForeignKey(td => td.FromStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WorkflowStatusTransition>()
                .HasOne(td => td.ToStatus)
                .WithMany(t => t.PreviousStatuses)
                .HasForeignKey(td => td.ToStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WorkflowStatusTransition>()
                .HasOne(st => st.AuthorizedRole)
                .WithMany(r => r.StatusTransitions)
                .HasForeignKey(ws => ws.AuthorizedRoleId)
                .OnDelete(DeleteBehavior.Restrict);


            // Message
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(s => s.MessagesSent)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(s => s.MessagesReceived)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);


        }

    }
}

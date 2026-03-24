using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Web.Models.Account;

namespace TicketSystem.Web.Models
{
    public class AppDbContext: IdentityDbContext<AppUser, AppRole, string,
                                                IdentityUserClaim<string>, AppUserRole, 
                                                IdentityUserLogin<string>, 
                                                IdentityRoleClaim<string>,
                                                IdentityUserToken<string>>
    {

        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            base.OnModelCreating(modelBuilder);

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
        }

    }
}

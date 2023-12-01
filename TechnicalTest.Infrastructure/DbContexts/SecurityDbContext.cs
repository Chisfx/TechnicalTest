using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using TechnicalTest.Domain.Entities;
using TechnicalTest.Application.Interfaces.Contexts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace TechnicalTest.Infrastructure.DbContexts
{
    public class SecurityDbContext : 
        IdentityDbContext<User,IdentityRole,string,IdentityUserClaim<string>,IdentityUserRole<string>,IdentityUserLogin<string>,IdentityRoleClaim<string>,IdentityUserToken<string>>, 
        ISecurityDbContext
    {
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options)
        {
           
        }
        public IDbConnection Connection => Database.GetDbConnection();
        public bool HasChanges => ChangeTracker.HasChanges();
        public DatabaseFacade DataBase => this.Database;
      
        public DbSet<User> User { get; set; }

        public async Task MigrateAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await base.Database.MigrateAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("Scurity");

            builder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users");
                entity.HasKey(e => e.Id);
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
    }
}

using Infrastructure.Abstracts;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities.Membership;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataAccessLayer.Migrations
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken> //DbContext
    {
        private readonly IIdentityService identityService;

        public DataContext(DbContextOptions<DataContext> options, IIdentityService? identityService = null)
            :base(options)
        {
            if (identityService == null)
                throw new ArgumentNullException(nameof(identityService), "IdentityService cannot be null");


            this.identityService = identityService;
        }

        //public DataContext(DbContextOptions options, IIdentityService identityService)
        //    : base(options)
        //{
        //    if (identityService == null)
        //        throw new ArgumentNullException(nameof(identityService), "IdentityService cannot be null");


        //    this.identityService = identityService;
        //}

        //public DbSet<Follow> Follows { get; set; }  // <-- Add this line

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changes = ChangeTracker.Entries<IAuditableEntity>();

            var userId = identityService?.GetPrincipialId(); //

            if (changes != null)
            {
                foreach (var entry in changes.Where(m => m.State == EntityState.Added
                || m.State == EntityState.Modified
                || m.State == EntityState.Deleted))
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatedBy = identityService.GetPrincipialId();
                            entry.Entity.CreatedAt = DateTime.UtcNow;
                            break;
                        case EntityState.Modified:
                            entry.Property(m => m.CreatedBy).IsModified = false;
                            entry.Property(m => m.CreatedAt).IsModified = false;
                            entry.Entity.LastModifiedBy = identityService.GetPrincipialId();
                            entry.Entity.LastModifiedAt = DateTime.UtcNow;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            entry.Property(m => m.CreatedBy).IsModified = false;
                            entry.Property(m => m.CreatedAt).IsModified = false;
                            entry.Property(m => m.LastModifiedBy).IsModified = false;
                            entry.Property(m => m.LastModifiedAt).IsModified = false;
                            //entry.Entity.DeletedBy = identityService.GetPrincipialId();
                            entry.Entity.DeletedBy = userId; //
                            entry.Entity.DeletedAt = DateTime.UtcNow;
                            break;
                        default:
                            break;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
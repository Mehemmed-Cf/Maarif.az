using Domain.Models.Abstract;
using Domain.Models.Entities;
using Domain.Models.Entities.Membership;
using Infrastructure.Abstracts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Migrations
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken> //DbContext
    {
        private readonly IIdentityService identityService;
        // FIX: Added all missing DbSets. Without these EF cannot track, query,
        // or migrate these tables — it would throw at startup or on first query.
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<StudentGroup> StudentGroups { get; set; }
        public DbSet<LessonGroup> LessonGroups { get; set; }
        public DbSet<TeacherDepartment> TeacherDepartments { get; set; }


        public DataContext(DbContextOptions<DataContext> options, IIdentityService? identityService = null)
            :base(options)
        {
            if (identityService == null)
                throw new ArgumentNullException(nameof(identityService), "IdentityService cannot be null");


            this.identityService = identityService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            modelBuilder.Entity<StudentGroup>()
                .HasKey(sg => new { sg.StudentId, sg.GroupId });

            // Configure the Join Table
            modelBuilder.Entity<LessonGroup>()
                .HasKey(lg => new { lg.LessonId, lg.GroupId }); // Composite Key

            modelBuilder.Entity<LessonGroup>()
                .HasOne(lg => lg.Lesson)
                .WithMany(l => l.LessonGroups)
                .HasForeignKey(lg => lg.LessonId);

            modelBuilder.Entity<LessonGroup>()
                .HasOne(lg => lg.Group)
                .WithMany(g => g.LessonGroups)
                .HasForeignKey(lg => lg.GroupId);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changes = ChangeTracker.Entries<IAuditableEntity>();

            var userId = identityService?.GetPrincipialId();

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
                            entry.Entity.DeletedBy = userId;
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
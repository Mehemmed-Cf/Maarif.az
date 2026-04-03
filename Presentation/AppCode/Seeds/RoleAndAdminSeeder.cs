using DataAccessLayer.Migrations;
using Domain.Models.Entities.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class RoleAndAdminSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var db = serviceProvider.GetRequiredService<DataContext>();

        // ── Seed roles ────────────────────────────────────────────
        string[] roles = { "SUPERADMIN", "STUDENT", "TEACHER" };

        foreach (var roleName in roles)
        {
            bool exists = await db.Set<AppRole>()
                .AnyAsync(r => r.NormalizedName == roleName.ToUpper());

            if (!exists)
            {
                await db.Set<AppRole>().AddAsync(new AppRole
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                });
            }
        }

        await db.SaveChangesAsync();

        // ── Seed superadmin user ──────────────────────────────────
        const string adminUserName = "superadmin";
        const string adminEmail = "admin@lms.edu.az";

        bool userExists = await db.Set<AppUser>()
            .AnyAsync(u => u.NormalizedEmail == adminEmail.ToUpper());

        if (!userExists)
        {
            var hasher = new PasswordHasher<AppUser>();

            var admin = new AppUser
            {
                UserName = adminUserName,
                NormalizedUserName = adminUserName.ToUpper(),
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

            await db.Set<AppUser>().AddAsync(admin);
            await db.SaveChangesAsync();

        }

        // Ensure SUPERADMIN role is assigned regardless of when the user was created
        var existingAdmin = await db.Set<AppUser>()
            .FirstOrDefaultAsync(u => u.NormalizedEmail == adminEmail.ToUpper());

        if (existingAdmin != null)
        {
            var superAdminRole = await db.Set<AppRole>()
                .FirstOrDefaultAsync(r => r.NormalizedName == "SUPERADMIN");

            if (superAdminRole != null)
            {
                var roleAssigned = await db.Set<AppUserRole>()
                    .AnyAsync(ur => ur.UserId == existingAdmin.Id && ur.RoleId == superAdminRole.Id);

                if (!roleAssigned)
                {
                    await db.Set<AppUserRole>().AddAsync(new AppUserRole
                    {
                        UserId = existingAdmin.Id,
                        RoleId = superAdminRole.Id
                    });
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
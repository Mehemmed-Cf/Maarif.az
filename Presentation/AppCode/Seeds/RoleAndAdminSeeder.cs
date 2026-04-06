using Domain.Models.Entities.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public static class RoleAndAdminSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        // ── Seed roles ────────────────────────────────────────────
        string[] roles = { "SUPERADMIN", "STUDENT", "TEACHER" };

        foreach (var roleName in roles)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                continue;

            var createRole = await roleManager.CreateAsync(new AppRole { Name = roleName });
            if (!createRole.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Role seed failed for {roleName}: {string.Join(", ", createRole.Errors.Select(e => e.Description))}");
            }
        }

        // ── Seed superadmin user ──────────────────────────────────
        const string adminUserName = "superadmin";
        const string adminEmail = "admin@lms.edu.az";

        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin is null)
        {
            admin = new AppUser
            {
                UserName = adminUserName,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createUser = await userManager.CreateAsync(admin, "Admin123!");
            if (!createUser.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Admin user seed failed: {string.Join(", ", createUser.Errors.Select(e => e.Description))}");
            }

            admin = await userManager.FindByEmailAsync(adminEmail)
                ?? throw new InvalidOperationException("Admin user was created but could not be reloaded.");
        }

        if (!await userManager.IsInRoleAsync(admin, "SUPERADMIN"))
        {
            var addRole = await userManager.AddToRoleAsync(admin, "SUPERADMIN");
            if (!addRole.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Assign SUPERADMIN failed: {string.Join(", ", addRole.Errors.Select(e => e.Description))}");
            }
        }
    }
}

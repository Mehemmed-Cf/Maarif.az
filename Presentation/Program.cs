using Application;
using Application.Mappings;
using Application.Services;
using DataAccessLayer.Migrations;
using FluentValidation.AspNetCore;
using Infrastructure.Abstracts;
using Infrastructure.Configurations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Presentation.AppCode.Diagnostics;
using Presentation.AppCode.DI;
using Presentation.AppCode.Pipeline;
using Presentation.AppCode.Seeds;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var agentDebugLogPath = Path.Combine(builder.Environment.ContentRootPath, "debug-b25443.log");
        Environment.SetEnvironmentVariable("MAARIF_DEBUG_LOG_PATH", agentDebugLogPath);

        builder.Host.UseServiceProviderFactory(new MaarifServiceProviderFactory());

        builder.Services.AddCors(cfg =>
        {

            cfg.AddPolicy("allowAll", p =>
            {

                p.AllowAnyHeader();
                p.AllowAnyMethod();
                p.AllowAnyOrigin();

            });

        });

        builder.Services.AddControllers(cfg =>
        {
            var policy = new AuthorizationPolicyBuilder()
                              .RequireAuthenticatedUser()
                              .Build();

            cfg.Filters.Add(new AuthorizeFilter(policy));
        });

        string? connectionString = builder.Configuration.GetConnectionString("cString");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'cString' was not found.");

        builder.Services.AddSingleton(_ => new AgentSqlDebugInterceptor(agentDebugLogPath));

        builder.Services.AddDbContext<DataContext>((sp, cfg) =>
        {
            cfg.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);

                sqlOptions.MigrationsHistoryTable("MigrationHistory");
            });
            cfg.AddInterceptors(sp.GetRequiredService<AgentSqlDebugInterceptor>());
        });

        builder.Services.AddCustomIdentity(builder.Configuration);

        // This scans the entire Assembly where your MappingProfile class is defined
        //builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
        // Clean and simple for the new version
        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(MappingProfile).Assembly);
        });

        builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(HeaderBinderBehaviour<,>));

        builder.Services.Configure<CryptoServiceOptions>(cfg => builder.Configuration.Bind(nameof(CryptoServiceOptions), cfg));


        //builder.Services.AddIdentity<AppUser, Microsoft.AspNetCore.Identity.IdentityRole<int>>(options => //User
        //{
        //    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //    options.Password.RequiredLength = 6;
        //    options.Password.RequireNonAlphanumeric = true;
        //});

        builder.Services.AddScoped<IIdentityService, FakeIdentityService>();

        builder.Services.AddScoped<IGovernmentIdentityService, FakeGovernmentIdentityService>();
        //builder.RegisterType<FakeGovernmentIdentityService>()
        //       .As<IGovernmentIdentityService>()
        //       .InstancePerLifetimeScope();


        builder.Services.AddSingleton<IFileService, FileService>();

        builder.Services.AddScoped<IWeekCalculatorService, WeekCalculatorService>();

        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        builder.Services.AddControllersWithViews();

        builder.Services.AddRouting(cfg => cfg.LowercaseUrls = true);

        //builder.Services.Configure<EmailServiceOptions>(builder.Configuration.GetSection("EmailServiceOptions"));
        //builder.Services.AddTransient<EmailService>();

        builder.Services.AddFluentValidationAutoValidation(cfg => cfg.DisableDataAnnotationsValidation = false);

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IApplicationReferance>());

        builder.Services.AddRazorPages();

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(cfg =>
        {
            cfg.IdleTimeout = TimeSpan.FromHours(24);
            cfg.Cookie.HttpOnly = true;
            cfg.Cookie.IsEssential = true;
        });

        builder.Services.AddScoped<DataSeeder>();

        var app = builder.Build();

        // #region agent log
        AgentDebugLog.Write(
            "H-LOGPATH",
            "Program.cs:start",
            "Debug logging initialized",
            new
            {
                contentRoot = builder.Environment.ContentRootPath,
                currentDir = Directory.GetCurrentDirectory(),
                configuredLogPath = agentDebugLogPath
            },
            agentDebugLogPath);
        Console.Error.WriteLine($"[Maarif.SqlDebug] logPath={agentDebugLogPath}");
        // #endregion

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var context = services.GetRequiredService<DataContext>();

                // 1. Fix the schema (Error 207)
                logger.LogInformation("Applying migrations...");
                await context.Database.MigrateAsync();

                // 2. Seed basic data
                logger.LogInformation("Seeding data...");
                var seeder = services.GetRequiredService<DataSeeder>();
                await seeder.SeedAsync();

                // 3. Seed Roles and Admin
                logger.LogInformation("Seeding roles and admin...");
                await RoleAndAdminSeeder.SeedAsync(services);

                logger.LogInformation("Database sync complete.");
            }
            catch (Exception ex)
            {
                // #region agent log
                AgentDebugLog.Write(
                    "H2-H4",
                    "Program.cs:database-init",
                    "Database initialization failed",
                    new
                    {
                        exMessage = ex.Message,
                        exType = ex.GetType().FullName,
                        inner = ex.InnerException?.Message,
                        stack = ex.ToString().Length > 4000 ? ex.ToString()[..4000] + "…" : ex.ToString()
                    },
                    agentDebugLogPath);
                // #endregion
                logger.LogError(ex, "Database initialization failed.");
                // If this fails, the app should probably stop
                throw;
            }
        }

        //using (var scope = app.Services.CreateScope())
        //{
        //    var services = scope.ServiceProvider;
        //    try
        //    {
        //        var context = services.GetRequiredService<DataContext>();

        //        // 1. Sync the schema (Fixes 'Invalid Column' errors)
        //        await context.Database.MigrateAsync();

        //        // 2. Seed the data
        //        var seeder = services.GetRequiredService<DataSeeder>();
        //        await seeder.SeedAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        var logger = services.GetRequiredService<ILogger<Program>>();
        //        logger.LogError(ex, "An error occurred during database migration or seeding.");
        //        // Optional: throw; if you want the container to restart on failure
        //    }
        //}

        //using (var scope = app.Services.CreateScope())
        //{
        //    await RoleAndAdminSeeder.SeedAsync(scope.ServiceProvider);
        //}

        //using (var scope = app.Services.CreateScope())
        //{
        //    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
        //    db.Database.Migrate();  // applies all pending migrations

        //    // your seeder below this if you have it
        //    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        //    await seeder.SeedAsync();
        //}

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        else
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseSession();

        app.UseCors("allowAll");

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapRazorPages();

        app.MapControllers();

        app.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

        app.MapControllerRoute(name: "default", pattern: "{controller=auth}/{action=login}/{id?}");

        app.Run();
    }
}
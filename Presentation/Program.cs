using Application;
using Application.Mappings;
using Application.Services;
using DataAccessLayer.Migrations;
using Domain.Models.Entities;
using Domain.Models.Entities.Membership;
using FluentValidation.AspNetCore;
using Infrastructure.Abstracts;
using Infrastructure.Configurations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Presentation.AppCode.DI;
using Presentation.AppCode.Pipeline;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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

        builder.Services.AddDbContext<DataContext>(cfg =>
        {
            cfg.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);

                sqlOptions.MigrationsHistoryTable("MigrationHistory");
            });
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

        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        builder.Services.AddControllersWithViews();

        builder.Services.AddRouting(cfg => cfg.LowercaseUrls = true);

        //builder.Services.Configure<EmailServiceOptions>(builder.Configuration.GetSection("EmailServiceOptions"));
        //builder.Services.AddTransient<EmailService>();

        builder.Services.AddFluentValidationAutoValidation(cfg => cfg.DisableDataAnnotationsValidation = false);

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IApplicationReferance>());

        builder.Services.AddRazorPages();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            dbContext.Database.Migrate();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        else
        {
            app.UseDeveloperExceptionPage();
        }

        //using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        //{
        //    await RoleAndAdminSeeder.SeedAsync(scope.ServiceProvider);
        //}

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseCors("allowAll");

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapRazorPages();

        app.MapControllers();

        app.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

        app.MapControllerRoute(name: "default", pattern: "{controller=home}/{action=index}/{id?}");

        using (var scope = app.Services.CreateScope())
        {

            await RoleAndAdminSeeder.SeedAsync(scope.ServiceProvider);
        }


        app.Run();
    }
}



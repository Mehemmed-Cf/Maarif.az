using Application.Services;
using DataAccessLayer.Migrations;
using Domain.Models.Entities.Membership;
using Infrastructure.Abstracts;
using Infrastructure.Configurations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Presentation.AppCode.DI;
using Presentation.AppCode.Pipeline;
using Application;

internal class Program
{
    private static void Main(string[] args)
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

        builder.Services.AddDbContext<DataContext>(cfg => //DbContext, 
        {
            string cs = builder.Configuration.GetConnectionString("cString");

            cfg.UseSqlServer(cs, opt =>
            {
                opt.MigrationsHistoryTable("MigrationHistory");
            });
        });

        builder.Services.AddCustomIdentity(builder.Configuration);

        builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(HeaderBinderBehaviour<,>));

        builder.Services.Configure<CryptoServiceOptions>(cfg => builder.Configuration.Bind(nameof(CryptoServiceOptions), cfg));

        builder.Services.AddIdentity<AppUser, Microsoft.AspNetCore.Identity.IdentityRole>(options => //User
        {
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = true;
        });

        builder.Services.AddScoped<IIdentityService, FakeIdentityService>();

        builder.Services.AddSingleton<IFileService, FileService>();

        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        builder.Services.AddControllersWithViews();

        builder.Services.AddRouting(cfg => cfg.LowercaseUrls = true);

        //builder.Services.Configure<EmailServiceOptions>(builder.Configuration.GetSection("EmailServiceOptions"));
        //builder.Services.AddTransient<EmailService>();

        /*builder.Services.AddFluentValidationAutoValidation(cfg => cfg.DisableDataAnnotationsValidation = false);*/

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IApplicationReferance>());

        builder.Services.AddRazorPages();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            app.UseDeveloperExceptionPage(); //
        }

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

        app.Run();
    }
}
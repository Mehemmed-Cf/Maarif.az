using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Domain.Models.Entities.Membership;
using DataAccessLayer.Migrations;

namespace Presentation.AppCode.Pipeline
{
    static class IdentityInjection
    {
        internal static IServiceCollection AddCustomIdentity(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddIdentityCore<AppUser>()
             .AddRoles<AppRole>()
             .AddSignInManager()
             .AddDefaultTokenProviders()
             .AddEntityFrameworkStores<DataContext>();

            services.Configure<IdentityOptions>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                // cfg.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                cfg.Lockout.AllowedForNewUsers = true;
                cfg.Lockout.MaxFailedAccessAttempts = 3;

                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 1;
                cfg.Password.RequiredLength = 3;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "MyAppAuthCookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                //options.LoginPath = "/Login";
                //options.AccessDeniedPath = "/NotAllowed";
                options.LoginPath = "/auth/login";
                options.AccessDeniedPath = "/auth/accessdenied";
                options.SlidingExpiration = true;
            });

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        //options.LoginPath = "/Login";
            //        //options.AccessDeniedPath = "/NotAllowed";
            //        options.LoginPath = "/auth/login";
            //        options.AccessDeniedPath = "/auth/login";
            //    });

            services.AddAuthentication(IdentityConstants.ApplicationScheme) // Use Identity's scheme name
                .AddCookie(IdentityConstants.ApplicationScheme, options => // Explicitly name it
                {
                    options.Cookie.Name = "MyAppAuthCookie";
                    options.Cookie.HttpOnly = true;
                    options.LoginPath = "/auth/login";
                    options.AccessDeniedPath = "/auth/accessdenied";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.SlidingExpiration = true;
                });

            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IClaimsTransformation, AppClaimsTransformation>();
            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, UserClaimsPrincipalFactory<AppUser, AppRole>>(); //

            services.AddAuthorization(cfg =>
            {
                // Register scanned policies first; RequireSuperAdminRole is re-applied below so it stays RequireRole(SUPERADMIN).
                foreach (var item in AppClaimsTransformation.policies)
                {
                    cfg.AddPolicy(item, p =>
                    {
                        p.RequireClaim(item, "1"); //

                        p.RequireAssertion(handler => handler.User.IsInRole("SUPERADMIN") || handler.User.HasClaim(item, "1"));

                        p.RequireAssertion(handler => handler.User.HasClaim(item, "1")); //
                    });
                }

                cfg.AddPolicy("RequireSuperAdminRole", policy => policy.RequireRole("SUPERADMIN"));
            });

            return services;
        }
    }
}

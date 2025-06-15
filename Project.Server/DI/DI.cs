using Project.Infra.Data;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;
using Project.Server.Identity;
using Project.Server.Configuration;
using Project.Domain.Entities;

namespace Project.Server.DI;
public static class ConfigureServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        });

        services.AddAuthorizationBuilder();

        services.AddCascadingAuthenticationState();

        services.AddIdentityCore<ProjectUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequiredUniqueChars = 1;
        })
            .AddEntityFrameworkStores<ProjectContext>()
            .AddErrorDescriber<PortugueseIdentityErrorDescriber>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.SlidingExpiration = false;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
            options.Cookie.IsEssential = true;
            options.Cookie.MaxAge = TimeSpan.FromHours(8);
            options.Events.OnRedirectToLogin = context =>
            {
                var requestPath = context.Request.Path;

                if (requestPath.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/problem+json";

                    var problemDetails = new ProblemDetails
                    {
                        Title = "Unauthorized",
                        Status = StatusCodes.Status401Unauthorized,
                        Detail = "You are not authorized to access this resource",
                    };

                    var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
                    return context.Response.WriteAsync(problemDetailsJson);
                }

                context.Response.Redirect($"/login?isForbidden=true&returnUrl={Uri.EscapeDataString(requestPath)}");
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAccessDenied = context =>
            {
                var requestPath = context.Request.Path;

                if (requestPath.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/problem+json";

                    var problemDetails = new ProblemDetails
                    {
                        Title = "Forbidden",
                        Status = StatusCodes.Status403Forbidden,
                        Detail = "You do not have permission to access this resource",
                    };

                    var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
                    return context.Response.WriteAsync(problemDetailsJson);
                }

                context.Response.Redirect($"/login?isForbidden=true&returnUrl={Uri.EscapeDataString(requestPath)}");
                return Task.CompletedTask;
            };
        });

        services.ConfigureOptions<ConfigureSecurityStampOptions>();

        var applicationAssembly = Assembly.GetAssembly(typeof(Application.DI.ConfigureServices)) ?? throw new AppDomainUnloadedException();


        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddHttpContextAccessor();

        services.AddEndpointsApiExplorer();

        var supportedCultures = new[]
        {
            new CultureInfo("en-US"),
            new CultureInfo("pt-BR"),
        };
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("pt-BR");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        return services;
    }
}

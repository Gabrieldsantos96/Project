using Project.Application.Features.User.Queries;
using Project.Application.Graphql;
using Project.Application.Graphql.Schema;
using Project.Application.Services;
using Project.Domain.Interfaces.Infra;
using AppAny.HotChocolate.FluentValidation;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Project.Application.EntryPoints.User.Mutations;

namespace Project.Application.DI;
public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<HtmlRenderer>();
        services.AddScoped<RazorTemplateRenderer>();
        services.AddScoped<IGraphQL, Executor>();
        services.AddScoped<ICreateAccountResolver, CreateAccountResolver>();
        services.AddScoped<IAuthenticateUserResolver, AuthenticateUserResolver>();
        services.AddScoped<IUserChangePasswordResolver, UserChangePasswordResolver>();
        services.AddScoped<IResetPasswordResolver, ResetPasswordResolver>();
        services.AddScoped<IChangeJobRoleResolver, ChangeJobRoleResolver>();
        services.AddScoped<IResetPasswordRequestResolver, ResetPasswordRequestResolver>();
        services.AddScoped<IGetUserProfileResolver, GetUserProfileResolver>();
        services.AddScoped<ILogoutResolver, LogoutResolver>();

        services
            .AddGraphQLServer()
            .AddAuthorization()
            .AddFiltering()
            .AddSorting()
            .AddProjections()
            .AddFluentValidation(s => s.UseDefaultErrorMapper())
            .ConfigBuilder();

        return services;
    }

    public static void ConfigBuilder(this IRequestExecutorBuilder builder)
    {
        AddSchemas(builder);
        AddResolvers(builder);
    }

    public static void AddSchemas(IRequestExecutorBuilder builder)
    {
        builder
            .AddType<EntitySchema>()
            .AddType<TenantSchema>()
            .AddType<CompanySchema>()
            .AddType<TenantBadgeSchema>()
            .AddType<EmployeeSchema>()
            .AddType<ProjectUserSchema>()
            .AddType<UserProfileDtoSchema>()
            .AddType<EmployeeDtoSchema>()
            .AddType<BadgeTypeSchema>();
    }

    public static void AddResolvers(this IRequestExecutorBuilder builder)
    {
        builder
            .AddQueryType<Queries>()
            .AddMutationType<Mutations>();
    }
}
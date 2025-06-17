using AppAny.HotChocolate.FluentValidation;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Project.Application.EntryPoints.User.Mutations;
using Project.Application.EntryPoints.Workflow.Mutations;
using Project.Application.EntryPoints.Workflow.Queries;
using Project.Application.Features.User.Queries;
using Project.Application.Graphql;
using Project.Application.Graphql.Schema;
using Project.Application.Interceptors;
using Project.Application.Services;

namespace Project.Application.DI;
public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<HtmlRenderer>();
        services.AddScoped<RazorTemplateRenderer>();
        services.AddScoped<ITokenHandler, TokenHandler>();
        services.AddScoped<ITokenValidator, TokenValidator>();
        services.AddScoped<ICreateAccountResolver, CreateAccountResolver>();
        services.AddScoped<ISigninResolver, SigninResolver>();
        services.AddScoped<ISignoutResolver, SignoutResolver>();
        services.AddScoped<IRefreshTokenResolver, RefreshTokenResolver>();
        services.AddScoped<IUserChangePasswordResolver, UserChangePasswordResolver>();
        services.AddScoped<IResetPasswordResolver, ResetPasswordResolver>();
        services.AddScoped<IChangeJobRoleResolver, ChangeJobRoleResolver>();
        services.AddScoped<IResetPasswordRequestResolver, ResetPasswordRequestResolver>();
        services.AddScoped<IGetUserProfileResolver, GetUserProfileResolver>();

        services.AddScoped<IPurchaseOrderResolver, CreatePurchaseOrderResolver>();
        services.AddScoped<IMatchingResolver, CreateMatchingResolver>();
        services.AddScoped<IFindWorkflowResolver, FindWorkflowByRefResolver>();
        services.AddScoped<IListWorkflowResolvers, ListWorkflowsResolvers>();

        services
            .AddGraphQLServer()
            .AddAuthorization()
            .AddFiltering()
            .AddSorting()
            .AddProjections()
            .AddRavenFiltering()
  	        .AddRavenProjections()
  	        .AddRavenSorting()
  	        .AddRavenPagingProviders()
            .AddHttpRequestInterceptor<HttpRequestInterceptor>()
            .AddFluentValidation(s => s.UseDefaultErrorMapper())
            .ConfigBuilder();

        return services;
    }
    public static void ConfigBuilder(this IRequestExecutorBuilder builder)
    {
        AddSchemas(builder);
        AddInputs(builder);
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
            .AddType<BadgeTypeSchema>()
            .AddType<WorkflowInterfaceTypeSchema>()
            .AddType<WorkflowMatchingTypeSchema>()
            .AddType<WorkflowPurchaseOrderTypeSchema>();
    }
    public static void AddInputs(IRequestExecutorBuilder builder)
    {
        builder
           .AddType<WorkflowBaseInputTypeSchema>()
           .AddType<WorkflowMatchingInputTypeSchema>()
           .AddType<WorkflowPurchaseInputOrderTypeSchema>();

    }
    public static void AddResolvers(IRequestExecutorBuilder builder)
    {
        builder
            .AddQueryType<Queries>()
            .AddMutationType<Mutations>();
    }
}
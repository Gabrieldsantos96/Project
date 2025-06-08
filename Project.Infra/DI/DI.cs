using Project.Domain.Interfaces.Infra;
using Project.Infra.Data;
using Project.Infra.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Project.Infra.DI;
public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = "Server=(localdb)\\Local;Database=Project;Trusted_Connection=True;";

        services.AddDbContextFactory<ProjectContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlBuilder =>
            {
                sqlBuilder.EnableRetryOnFailure(3);
                sqlBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });

        services.AddSingleton<IMailService, MailService>();

        services.AddScoped<IClaimsService, ClaimsService>();

        services.AddScoped<IProjectContextFactory, ProjectContextFactory>();


        services.AddDataProtection()
            .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
            {
                EncryptionAlgorithm = EncryptionAlgorithm.AES_256_GCM,
                ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
            })
            .PersistKeysToDbContext<ProjectContext>();

        services.AddMemoryCache();
    }
}

using Project.Infra.Data;

namespace Project.Server.Utils;
public class Seeding
{
    public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider, IWebHostEnvironment environment)
    {
        try
        {
            await Task.Delay(0);

            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProjectContext>();

        }
        catch (Exception e)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Seeding>>();

            logger.LogCritical(e, "Erro ao migrar ou realizar seeding do banco de dados.");
        }
    }

}
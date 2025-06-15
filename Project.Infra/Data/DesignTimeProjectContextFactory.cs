using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace Project.Infra.Data;
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProjectContext>
{
    public ProjectContext CreateDbContext(string[] args)
    {
        var connectionString = "Server=(localdb)\\Local;Database=Project;Trusted_Connection=True;";

        var optionsBuilder = new DbContextOptionsBuilder<ProjectContext>();
        optionsBuilder.UseSqlServer(connectionString, builder =>
        {
            builder.EnableRetryOnFailure(3);
            builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        });

        return new ProjectContext(optionsBuilder.Options, null!);
    }
}

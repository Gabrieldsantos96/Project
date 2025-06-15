using Project.Domain.Interfaces.Infra;
using Microsoft.EntityFrameworkCore;

namespace Project.Infra.Data;
public sealed class ProjectContextFactory(IDbContextFactory<ProjectContext> dbContextFactory, IClaimsService _claimsService) : IProjectContextFactory
{
    public IProjectContext CreateDbContext()
    {
        var tenantId = _claimsService.GetTenantId();

        var context = dbContextFactory.CreateDbContext();

        context.SetTenantId(tenantId);

        return context;
    }

    public IProjectContext CreateDbContext(int tenantId)
    {
        var context = dbContextFactory.CreateDbContext();

        context.SetTenantId(tenantId);

        return context;
    }

    public Task<IProjectContext> CreateDbContextAsync()
    => Task.FromResult(CreateDbContext());

    public Task<IProjectContext> CreateDbContextAsync(int tenantId)
        => Task.FromResult(CreateDbContext(tenantId));
}
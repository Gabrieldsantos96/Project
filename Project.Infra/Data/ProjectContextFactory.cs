using Project.Domain.Interfaces.Infra;
using Microsoft.EntityFrameworkCore;

namespace Project.Infra.Data;
public sealed class ProjectContextFactory(IDbContextFactory<ProjectContext> dbContextFactory, IClaimsService _claimsService) : IProjectContextFactory
{
    public async Task<IProjectContext> CreateDbContextAsync()
    {
        var tenantId = _claimsService.GetTenantId();

        var context = await dbContextFactory.CreateDbContextAsync();

        context.SetTenantId(tenantId);

        return context;
    }

    public async Task<IProjectContext> CreateDbContextAsync(int tenantId)
    {
        var context = await dbContextFactory.CreateDbContextAsync();

        context.SetTenantId(tenantId);

        return context;
    }
}
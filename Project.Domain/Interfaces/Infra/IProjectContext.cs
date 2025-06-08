using Project.Domain.Entities;
using Project.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Project.Domain.Interfaces.Infra;
public interface IProjectContext : IDisposable, IAsyncDisposable
{
    public DbSet<ProjectUser> Users { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Department> Deparments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<TenantBadge> TenantBadges { get; set; }


    DbSet<T> Set<T>() where T : class;

    EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DatabaseFacade Database { get; }
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
        where TEntity : class;
}

public interface IProjectContextFactory
{
    Task<IProjectContext> CreateDbContextAsync();
}

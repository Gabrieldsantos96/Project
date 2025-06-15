using Project.Domain.Entities;
using Project.Domain.Interfaces.Infra;
using Project.Shared.Consts;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using System.Security.Claims;

namespace Project.Infra.Data;
public sealed class ProjectContext(
    DbContextOptions<ProjectContext> options,
    IHttpContextAccessor? httpContextAccessor)
    : IdentityDbContext<ProjectUser, ProjectRole, int>(options), IDataProtectionKeyContext, IProjectContext
{
    private readonly int _userId = int.TryParse(httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out int result) ? result : 0;

    private int _tenantId = int.TryParse(httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimsConsts.TenantId)?.Value, out int result)
        ? result
        : 0;
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Department> Deparments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<TenantBadge> TenantBadges { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<WorkflowBase> Workflows { get; set; }
    public void SetTenantId(int tenantId)
    {
        _tenantId = tenantId;
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region DbConfig
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        var entityTypes = modelBuilder.Model.GetEntityTypes()
            .Where(t => typeof(ITenant).IsAssignableFrom(t.ClrType) && t.BaseType == null);

        foreach (var entityType in entityTypes)
        {
            var method = typeof(ProjectContext).GetMethod(nameof(SetGlobalQueryFilter), BindingFlags.NonPublic | BindingFlags.Instance)
                ?.MakeGenericMethod(entityType.ClrType);

            method?.Invoke(this, [modelBuilder]);
        }
        #endregion

        #region Relations
        modelBuilder.Entity<ProjectUser>()
            .HasMany(u => u.Employees)
            .WithOne(j => j.User)
            .HasForeignKey(j => j.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tenant>()
            .HasMany(t => t.Companies)
            .WithOne(c => c.Tenant)
            .HasForeignKey(c => c.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Tenant>()
           .HasMany(t => t.Employees)
           .WithOne(c => c.Tenant)
           .HasForeignKey(c => c.TenantId)
           .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Tenant>()
            .HasMany(t => t.TenantBadges)
            .WithOne(c => c.Tenant)
            .HasForeignKey(c => c.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Company>()
            .HasMany(t => t.Departments)
            .WithOne(c => c.Company)
            .HasForeignKey(c => c.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

    }

    private void SetGlobalQueryFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : class, ITenant
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(e => e.TenantId == _tenantId);
    }
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var userId = _userId.ToString();

        var entityEntries = ChangeTracker.Entries<Entity>().ToList();
        foreach (var entry in entityEntries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = userId;
            }
            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = userId;
            }
        }

        var tenantEntries = ChangeTracker.Entries<ITenant>().ToList();
        foreach (var entry in tenantEntries)
        {
            if (entry.State is EntityState.Added or EntityState.Modified && entry.Entity.TenantId == 0)
            {
                entry.Entity.TenantId = _tenantId;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}

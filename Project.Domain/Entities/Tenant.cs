using Project.Domain.Entities.User;
using Project.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Domain.Entities;

public interface ITenant
{
    [ForeignKey(nameof(TenantId))]
    public int TenantId { get; set; }
    public Tenant Tenant { get; set; }
}

public sealed class Tenant : Entity
{
    public Guid RefId { get; set; } = Guid.NewGuid();

    public string Name { get; private set; } = null!;

    public IEnumerable<Company> Companies { get; set; } = new List<Company>();

    public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();

    public IEnumerable<TenantBadge> TenantBadges { get; set; } = new List<TenantBadge>();
}

public class TenantBadge : Entity, ITenant
{
    public Guid RefId { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(TenantId))]
    public int TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    public BadgeType BadgeType { get; set; }
    public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();
}

public class Company : Entity, ITenant
{
    public Guid RefId { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(TenantId))]
    public int TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    public string Name { get; set; } = null!;
    public List<Department> Departments { get; set; } = new List<Department>();
}


public class Department : Entity, ITenant
{
    public Guid RefId { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(TenantId))]
    public int TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;

    [ForeignKey(nameof(CompanyId))]
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    public string Name { get; set; } = null!;
}

public class Employee : Entity, ITenant
{
    public Guid RefId { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(TenantId))]
    public int TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    public string JobName { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public int UserId { get; set; }
    public ProjectUser User { get; set; } = null!;
    public IEnumerable<TenantBadge> TenantBadges { get; set; } = new List<TenantBadge>();

}



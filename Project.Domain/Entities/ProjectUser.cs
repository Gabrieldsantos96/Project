using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Project.Domain.Entities;

[Index(nameof(RefId))]
[Index(nameof(Email))]
public class ProjectUser : IdentityUser<int>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Guid RefId { get; set; } = Guid.NewGuid();

    [MaxLength(500)]
    public string Name { get; set; } = null!;
    public List<Employee> Employees { get; set; } = new List<Employee>();
    public int? EmployeeSelectedId { get; set; }
}

public class ProjectRole : IdentityRole<int>
{
}


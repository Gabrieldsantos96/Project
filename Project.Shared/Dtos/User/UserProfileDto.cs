using Project.Shared.Enums;

namespace Project.Shared.Dtos.User;
public sealed class UserProfileDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public IEnumerable<EmployeeDto> EmployeeDtos { get; set; } = null!;
    public Guid? EmployeeSelectedRefId { get; set; }
}

public sealed class EmployeeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid TenantId { get; set; }
    public IEnumerable<BadgeType> BadgeDtos { get; set; } = new List<BadgeType>();
}


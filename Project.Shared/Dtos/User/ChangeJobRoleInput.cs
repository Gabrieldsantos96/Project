using System.ComponentModel.DataAnnotations;

namespace Project.Shared.Dtos.User;
public sealed class ChangeJobRoleInput
{

    [Required]
    public Guid EmployeeId { get; set; }
}
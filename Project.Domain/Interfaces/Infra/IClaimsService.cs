using Project.Domain.Entities;
using System.Security.Claims;

namespace Project.Domain.Interfaces.Infra;

public interface IClaimsService
{
    int GetUserId();
    Guid GetUserRefId();
    string? GetUsername();
    int GetTenantId();
    Guid GetTenantRefId();
    int GetEmployeeId();
    public Guid GetEmployeeRefId();
    string? GetRole();
    bool IsInRole(string role);
    List<Claim>? GetAllClaims();
    List<Claim> GenerateClaims(ProjectUser user, Employee? selectedEmployee);
}
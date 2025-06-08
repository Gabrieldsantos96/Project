using Project.Domain.Entities;
using Project.Domain.Entities.User;
using Project.Domain.Interfaces.Infra;
using Project.Shared.Consts;
using Project.Shared.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Project.Infra.Services;
public class ClaimsService(IHttpContextAccessor? httpContextAccessor) : IClaimsService
{

    public int GetUserId()
    {
        var userIdString = httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        return userIdString == null ? 0 : int.Parse(userIdString);
    }

    public Guid GetUserRefId()
    {
        var userIdString = httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimsConsts.UserRefId);

        return userIdString == null ? Guid.Empty : Guid.Parse(userIdString);
    }

    public string? GetUsername()
    {
        return httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimsConsts.Username);
    }

    public int GetTenantId()
    {
        return int.TryParse(httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimsConsts.TenantId)?.Value, out int result)
            ? result
            : 0;
    }

    public Guid GetTenantRefId()
    {
        return Guid.TryParse(httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimsConsts.TenantRefId)?.Value, out Guid result)
            ? result
            : Guid.Empty;
    }

    public int GetEmployeeId()
    {
        return int.TryParse(httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimsConsts.EmployeeId)?.Value, out int result)
            ? result
            : 0;
    }

    public Guid GetEmployeeRefId()
    {
        return Guid.TryParse(httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimsConsts.EmployeeRefId)?.Value, out Guid result)
            ? result
            : Guid.Empty;
    }

    public string? GetRole()
    {
        return httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
    }


    public bool IsInRole(string role)
    {
        return httpContextAccessor?.HttpContext?.User.IsInRole(role) ?? false;
    }

    public List<Claim>? GetAllClaims()
    {
        return httpContextAccessor?.HttpContext?.User.Claims.ToList();
    }

    public List<Claim> GenerateClaims(ProjectUser user, Employee? selectedEmployee)
    {
        selectedEmployee ??= user.Employees.FirstOrDefault();

        var claims = new List<Claim>
        {
            new Claim(ClaimsConsts.UserRefId, user.RefId.ToString()),
            new Claim(ClaimsConsts.UserId, user.Id.ToString()),
        };

        if (selectedEmployee is not null)
        {
            claims.Add(new Claim(ClaimsConsts.EmployeeRefId, selectedEmployee.RefId.ToString()));
            claims.Add(new Claim(ClaimsConsts.EmployeeId, selectedEmployee.Id.ToString()));
            claims.Add(new Claim(ClaimsConsts.TenantRefId, selectedEmployee.Tenant.RefId.ToString()));
            claims.Add(new Claim(ClaimsConsts.TenantId, selectedEmployee.TenantId.ToString()));

            var roles = selectedEmployee.TenantBadges.Select(s => s.BadgeType);

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    switch (role)
                    {
                        case BadgeType.Developer:
                            claims.Add(new Claim(ClaimTypes.Role, RoleConsts.Developer));
                            break;

                        case BadgeType.Manager:
                            claims.Add(new Claim(ClaimTypes.Role, RoleConsts.Manager));
                            break;

                        case BadgeType.Analyst:
                            claims.Add(new Claim(ClaimTypes.Role, RoleConsts.Analyst));
                            break;
                        default:
                            throw new InvalidDataException("role doesn't exists");
                    }
                }
            }

        }

        return claims;
    }


}
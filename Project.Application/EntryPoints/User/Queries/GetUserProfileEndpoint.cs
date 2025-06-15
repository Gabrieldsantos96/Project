using Project.Domain.Interfaces.Infra;
using Project.Shared.Dtos.User;
using Project.Shared.Validations;
using Microsoft.EntityFrameworkCore;

namespace Project.Application.Features.User.Queries;
public interface IGetUserProfileResolver
{
    Task<UserProfileDto> GetUserProfileAsync(CancellationToken ct);
}
public sealed class GetUserProfileResolver(IClaimsService claimsService, IProjectContextFactory projectContextFactory) : IGetUserProfileResolver
{
    public async Task<UserProfileDto> GetUserProfileAsync(CancellationToken ct)
    {
        using var ctx = projectContextFactory.CreateDbContext();

        var userId = claimsService.GetUserId();

        var user = await ctx.Users.AsNoTracking()
                .IgnoreQueryFilters()
                .Include(s => s.Employees).ThenInclude(s => s.Tenant)
                .FirstOrDefaultAsync(u => u.Id == userId, ct)
            ?? throw new ArgumentException(ValidationMessages.DefaultAuthenticationError);

        var userDto = new UserProfileDto
        {
            Id = user.RefId,
            Email = user.Email!,
            UserName = user.UserName!,
            EmployeeDtos = user.Employees.Select(s => new EmployeeDto()
            {
                TenantId = s.Tenant.RefId,
                BadgeDtos = s.TenantBadges.Select(s => s.BadgeType),
                Id = s.RefId,
                Name = s.JobName
            })
        };

        var selectedEmployeeRefId = claimsService.GetEmployeeRefId();

        var employee = user.Employees.FirstOrDefault(s => s.RefId == selectedEmployeeRefId);

        if (employee != null)
        {
            userDto.EmployeeSelectedRefId = employee.RefId;

        }

        return userDto;
    }
}
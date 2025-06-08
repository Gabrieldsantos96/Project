using Project.Domain.Interfaces.Infra;
using Project.Shared.Dtos.User;
using Project.Shared.Validations;
using Microsoft.EntityFrameworkCore;

namespace Project.Application.Features.User.Queries;
public interface IGetUserProfileResolver
{
    Task<UserProfileDto> GetUserProfileAsync(CancellationToken ct);
}
public sealed class GetUserProfileResolver(
    IGraphQL _graphQL, IClaimsService _claimsService, IProjectContextFactory _projectContextFactory) : IGetUserProfileResolver
{
    public async Task<UserProfileDto> GetUserProfileAsync(CancellationToken ct)
    {
        await using var ctx = await _projectContextFactory.CreateDbContextAsync();

        var userId = _claimsService.GetUserId();

        var user = await _graphQL.ExecuteQueryAsync(
            ctx,
            async context => await context.Users.AsNoTracking()
                .IgnoreQueryFilters()
                .Include(s => s.Employees).ThenInclude(s => s.Tenant)
                .FirstOrDefaultAsync(u => u.Id == userId), ct)
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

        var selectedEmployeeRefId = _claimsService.GetEmployeeRefId();

        var employee = user.Employees.FirstOrDefault(s => s.RefId == selectedEmployeeRefId);

        if (employee != null)
        {
            userDto.EmployeeSelectedRefId = employee.RefId;

        }

        return userDto;
    }
}
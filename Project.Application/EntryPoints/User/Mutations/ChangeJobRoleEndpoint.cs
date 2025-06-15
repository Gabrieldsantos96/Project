using Project.Domain.Interfaces.Infra;
using Project.Shared.Dtos.User;
using Project.Shared.Validations;
using AppAny.HotChocolate.FluentValidation;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Domain.Entities;
using Project.Domain.Models;

namespace Project.Application.EntryPoints.User.Mutations;
public sealed class ChangeJobRoleInputValidator : AbstractValidator<ChangeJobRoleInput>
{
    public ChangeJobRoleInputValidator()
    {
        RuleFor(s => s.EmployeeId)
            .NotEmpty().WithMessage(ValidationMessages.FieldRequired);
    }
}
public interface IChangeJobRoleResolver
{
    Task<MutationResult<object>> ChangeJobRoleAsync(ChangeJobRoleInput input, CancellationToken ct);
}
public sealed class ChangeJobRoleResolver(
    SignInManager<ProjectUser> signInManager,
    IProjectContextFactory projectContextFactory,
    IClaimsService claimsService) : IChangeJobRoleResolver
{
    public async Task<MutationResult<object>> ChangeJobRoleAsync([UseFluentValidation, UseValidator<ChangeJobRoleInputValidator>] ChangeJobRoleInput input, CancellationToken ct)
    {
        var userRefId = claimsService.GetUserRefId();

        using var ctx = projectContextFactory.CreateDbContext();

        var user = await ctx.Users
                .FirstOrDefaultAsync(s => s.RefId == userRefId && s.Employees.Any(s => s.RefId == input.EmployeeId), ct)
            ?? throw new ArgumentException(ValidationMessages.FieldValueInvalid);

        var selectedEmployee = user.Employees.FirstOrDefault(s => s.RefId == input.EmployeeId)
            ?? throw new ArgumentException(ValidationMessages.FieldValueInvalid);

        user.EmployeeSelectedId = selectedEmployee.Id;

        var identityResult = await signInManager.UserManager.UpdateAsync(user);

        if (!identityResult.Succeeded)
        {
            throw new ArgumentException(ValidationMessages.DefaultAuthenticationError);
        }
        await signInManager.RefreshSignInAsync(user);

        var current = user.Employees.FirstOrDefault(s => s.Id == user.EmployeeSelectedId);

        var claims = claimsService.GenerateClaims(user, current);

        await signInManager.SignInWithClaimsAsync(user, true, claims);

        return MutationResult<object>.Ok("Cargo alterado com sucesso", new object());

    }
}
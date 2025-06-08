using Project.Shared.Models;
using Project.Domain.Entities.User;
using Project.Domain.Interfaces.Infra;
using Project.Shared.Dtos.User;
using Project.Shared.Validations;
using AppAny.HotChocolate.FluentValidation;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
    Task<MutationResult> ChangeJobRoleAsync(ChangeJobRoleInput input, CancellationToken ct);
}
public sealed class ChangeJobRoleResolver(
    SignInManager<ProjectUser> _signInManager,
    IGraphQL _graphQL,
    IClaimsService _claimsService) : IChangeJobRoleResolver
{
    public async Task<MutationResult> ChangeJobRoleAsync([UseFluentValidation, UseValidator<ChangeJobRoleInputValidator>] ChangeJobRoleInput input, CancellationToken ct)
    {
        var userRefId = _claimsService.GetUserRefId();



        var user = await _graphQL.ExecuteQueryAsync(
            async context => await context.Users
                .FirstOrDefaultAsync(s => s.RefId == userRefId && s.Employees.Any(s => s.RefId == input.EmployeeId))
            ?? throw new ArgumentException(ValidationMessages.FieldValueInvalid));

        var selectedEmployee = user.Employees.FirstOrDefault(s => s.RefId == input.EmployeeId)
            ?? throw new ArgumentException(ValidationMessages.FieldValueInvalid);

        user.EmployeeSelectedId = selectedEmployee.Id;

        var identityResult = await _signInManager.UserManager.UpdateAsync(user);

        if (!identityResult.Succeeded)
        {
            throw new ArgumentException(ValidationMessages.DefaultAuthenticationError);
        }

        await _signInManager.RefreshSignInAsync(user);

        var current = user.Employees.FirstOrDefault(s => s.Id == user.EmployeeSelectedId);

        var claims = _claimsService.GenerateClaims(user, current);

        await _signInManager.SignInWithClaimsAsync(user, true, claims);

        return MutationResult.Ok("Cargo alterado com sucesso");

    }
}
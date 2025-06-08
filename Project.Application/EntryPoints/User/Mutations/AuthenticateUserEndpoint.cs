using Project.Domain.Entities.User;
using Project.Domain.Interfaces.Infra;
using Project.Shared.Dtos.User;
using Project.Shared.Models;
using Project.Shared.Validations;
using AppAny.HotChocolate.FluentValidation;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Project.Application.EntryPoints.User.Mutations;
public sealed class AuthenticateUserInputValidator : AbstractValidator<UserAuthenticationInput>
{
    public AuthenticateUserInputValidator()
    {
        RuleFor(s => s.Email)
            .NotEmpty().WithMessage(ValidationMessages.DefaultAuthenticationError);

        RuleFor(s => s.Password)
            .NotEmpty().WithMessage(ValidationMessages.DefaultAuthenticationError);
    }
}
public interface IAuthenticateUserResolver
{
    Task<MutationResult> AuthenticateUserAsync(UserAuthenticationInput input, CancellationToken ct);
}
public sealed class AuthenticateUserResolver(SignInManager<ProjectUser> _signInManager,IGraphQL _graphQL, IClaimsService _claimsService) : IAuthenticateUserResolver
{
    public async Task<MutationResult> AuthenticateUserAsync(
        [UseFluentValidation, UseValidator<AuthenticateUserInputValidator>] UserAuthenticationInput input,
        CancellationToken ct)
    {

        var user = await _graphQL.ExecuteQueryAsync(
            async context => await context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == input.Email), ct)
            ?? throw new ArgumentException(ValidationMessages.DefaultAuthenticationError);

        _signInManager.AuthenticationScheme = IdentityConstants.ApplicationScheme;

        var result = await _signInManager.PasswordSignInAsync(user, input.Password!, false, false);

        if (result.Succeeded)
        {
            var current = user.Employees.FirstOrDefault(s => s.Id == user.EmployeeSelectedId);
            var claims = _claimsService.GenerateClaims(user, current);
            await _signInManager.SignInWithClaimsAsync(user, true, claims);

            return MutationResult.Ok("Usuário autenticado com sucesso");
        }

        if (result.IsNotAllowed) throw new Exception(ValidationMessages.IsNotAllowed);

        if (result.IsLockedOut) throw new ArgumentException(ValidationMessages.UserLockedOut);

        throw new ArgumentException(ValidationMessages.DefaultAuthenticationError);
    }
}
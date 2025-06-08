using Project.Shared.Models;
using Project.Domain.Entities.User;
using Project.Domain.Extensions;
using Project.Domain.Interfaces.Infra;
using Project.Shared.Dtos.User;
using Project.Shared.Validations;
using AppAny.HotChocolate.FluentValidation;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Project.Application.EntryPoints.User.Mutations;
public sealed class ChangePasswordInputValidator : AbstractValidator<ChangePasswordInput>
{
    public ChangePasswordInputValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage(ValidationMessages.DefaultAuthenticationError);

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(ValidationMessages.DefaultAuthenticationError)
            .MinimumLength(6).WithMessage("A nova senha deve ter pelo menos 6 caracteres");
    }
}
public interface IUserChangePasswordResolver
{
    Task<MutationResult> ChangePasswordAsync(ChangePasswordInput input, CancellationToken ct);
}
public sealed class UserChangePasswordResolver(IClaimsService _claimsService,IGraphQL _graphQL, UserManager<ProjectUser> _userManager) : IUserChangePasswordResolver
{
    public async Task<MutationResult> ChangePasswordAsync([UseFluentValidation, UseValidator<ChangePasswordInputValidator>] ChangePasswordInput input, CancellationToken ct)
    {
        var userRefId = _claimsService.GetUserRefId();
        
        var user = await _graphQL.ExecuteQueryAsync(
            async context => await context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.RefId == userRefId), ct)
            ?? throw new ArgumentException(ValidationMessages.DefaultAuthenticationError);

        var result = await _userManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);

        if (!result.Succeeded)
        {
            var error = result.Errors.Select(e => e.Description).FirstOrDefault();
            throw new DomainException(error ?? "Ocorreu um erro ao redefinir a sua senha");
        }

        return MutationResult.Ok("Senha alterada com sucesso");
    }
}
using Project.Domain.Extensions;
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
    Task<MutationResult<object>> ChangePasswordAsync(ChangePasswordInput input, CancellationToken ct);
}
public sealed class UserChangePasswordResolver(IClaimsService claimsService,UserManager<ProjectUser> userManager, IProjectContextFactory projectContextFactory) : IUserChangePasswordResolver
{
    public async Task<MutationResult<object>> ChangePasswordAsync([UseFluentValidation, UseValidator<ChangePasswordInputValidator>] ChangePasswordInput input, CancellationToken ct)
    {
        var userRefId = claimsService.GetUserRefId();

        using var ctx = projectContextFactory.CreateDbContext();

        var user = await ctx.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.RefId == userRefId,ct)
            ?? throw new ArgumentException(ValidationMessages.DefaultAuthenticationError);

        var result = await userManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);

        if (!result.Succeeded)
        {
            var error = result.Errors.Select(e => e.Description).FirstOrDefault();
            throw new DomainException(error ?? "Ocorreu um erro ao redefinir a sua senha");
        }

        return MutationResult<object>.Ok("Senha alterada com sucesso", new object());
    }
}
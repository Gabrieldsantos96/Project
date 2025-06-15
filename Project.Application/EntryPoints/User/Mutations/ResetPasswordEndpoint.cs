using Project.Domain.Extensions;
using Project.Domain.Interfaces.Infra;
using Project.Shared.Dtos.User;
using Project.Shared.Validations;
using AppAny.HotChocolate.FluentValidation;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;
using Project.Domain.Entities;
using Project.Domain.Models;

namespace Project.Application.EntryPoints.User.Mutations;
public sealed class ResetPasswordInputValidator : AbstractValidator<ResetPasswordInput>
{
    public ResetPasswordInputValidator()
    {
        RuleFor(x => x.EncodedEmail)
            .NotEmpty().WithMessage(ValidationMessages.DefaultAuthenticationError);

        RuleFor(x => x.EncodedToken)
            .NotEmpty().WithMessage(ValidationMessages.DefaultAuthenticationError);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationMessages.DefaultAuthenticationError)
            .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres");
    }
}
public interface IResetPasswordResolver
{
    Task<MutationResult<object>> ResetPasswordAsync(ResetPasswordInput input, CancellationToken ct);
}
public sealed class ResetPasswordResolver(UserManager<ProjectUser> userManager, IProjectContextFactory projectContextFactory, ILogger<ResetPasswordResolver> logger) : IResetPasswordResolver
{
    public async Task<MutationResult<object>> ResetPasswordAsync([UseFluentValidation, UseValidator<ResetPasswordInputValidator>] ResetPasswordInput input, CancellationToken ct)
    {
        var email = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input.EncodedEmail ?? throw new DomainException("E-mail inválido")));

        var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input.EncodedToken ?? throw new DomainException("Token inválido")));

        using var ctx = projectContextFactory.CreateDbContext();

        var user = await ctx.Users.AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Email == email, ct)
                ?? throw new ArgumentException(ValidationMessages.DefaultAuthenticationError);

        var result = await userManager.ResetPasswordAsync(user, token, input.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                logger.LogWarning("Falha ao trocar a senha do usuário com o e-mail {email}, Motivo: {reason}", user.Email, error.Description);
            }

            throw new DomainException("Falha ao definir a sua senha, tente novamente com outra senha ou entre em contato com o suporte");
        }

        user.EmailConfirmed = true;
        await userManager.UpdateAsync(user);

        return MutationResult<object>.Ok("", new object());

    }
}
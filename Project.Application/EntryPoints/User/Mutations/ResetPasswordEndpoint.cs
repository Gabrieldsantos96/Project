using Project.Domain.Entities.User;
using Project.Domain.Extensions;
using Project.Domain.Interfaces.Infra;
using Project.Shared.Dtos.User;
using Project.Shared.Models;
using Project.Shared.Validations;
using AppAny.HotChocolate.FluentValidation;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;

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
    Task<MutationResult> ResetPasswordAsync(ResetPasswordInput input, CancellationToken ct);
}
public sealed class ResetPasswordResolver(UserManager<ProjectUser> _userManager, IGraphQL _graphQL, ILogger<ResetPasswordResolver> _logger) : IResetPasswordResolver
{
    public async Task<MutationResult> ResetPasswordAsync([UseFluentValidation, UseValidator<ResetPasswordInputValidator>] ResetPasswordInput input, CancellationToken ct)
    {
        var email = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input.EncodedEmail ?? throw new DomainException("E-mail inválido")));

        var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input.EncodedToken ?? throw new DomainException("Token inválido")));

            var user = await _graphQL.ExecuteQueryAsync(
                async context => await context.Users.AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Email == email)
                ?? throw new ArgumentException(ValidationMessages.DefaultAuthenticationError));

        var result = await _userManager.ResetPasswordAsync(user, token, input.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogWarning("Falha ao trocar a senha do usuário com o e-mail {email}, Motivo: {reason}", user.Email, error.Description);
            }

            throw new DomainException("Falha ao definir a sua senha, tente novamente com outra senha ou entre em contato com o suporte");
        }

        user.EmailConfirmed = true;
        await _userManager.UpdateAsync(user);

        return MutationResult.Ok("Senha redefinida com sucesso.");

    }
}
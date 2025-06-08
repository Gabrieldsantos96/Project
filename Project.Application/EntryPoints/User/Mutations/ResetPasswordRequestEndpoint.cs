using Project.Shared.Models;
using Project.Application.Services;
using Project.Domain.Entities.User;
using Project.Domain.Interfaces.Infra;
using Project.Domain.Models;
using Project.Templates;
using Project.Shared.Dtos.User;
using Project.Shared.Validations;
using AppAny.HotChocolate.FluentValidation;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Project.Application.EntryPoints.User.Mutations;
public sealed class ResetPasswordRequestInputValidator : AbstractValidator<ResetPasswordInput>
{
    public ResetPasswordRequestInputValidator() { }
}
public interface IResetPasswordRequestResolver
{
    Task<MutationResult> ResetPasswordRequestAsync(ResetPasswordRequestInput input, CancellationToken ct);
}
public sealed class ResetPasswordRequestResolver(UserManager<ProjectUser> _userManager, IMailService _mailService, IGraphQL _graphQL, RazorTemplateRenderer _razorTemplateRenderer) : IResetPasswordRequestResolver
{
    public async Task<MutationResult> ResetPasswordRequestAsync([UseFluentValidation, UseValidator<ResetPasswordRequestInputValidator>] ResetPasswordRequestInput input, CancellationToken ct)
    {
        var user = await _graphQL.ExecuteQueryAsync(
        async context => await context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == input.Email), ct)
        ?? throw new ArgumentException(ValidationMessages.DefaultAuthenticationError);

        await SendForgotPasswordEmailAsync(user, new ForgotPasswordModel(user.Name));

        return MutationResult.Ok("Email de redefinição de senha enviado com sucesso. Verifique sua caixa de entrada ou spam.");

    }

    private async Task SendForgotPasswordEmailAsync(ProjectUser user, ForgotPasswordModel model)
    {
        model.Url += await GenerateResetPasswordToken(user);

        var templateHtml = await _razorTemplateRenderer.RenderAsync<ForgotPasswordModelTemplate>(model);

        await _mailService.SendMailAsync(new MailRequest
        {
            Subject = "PROJECT - Redefinição de Senha",
            ContactName = user.Name,
            ContactMail = user.Email!,
            BodyHtml = templateHtml
        });
    }

    private async Task<string> GenerateResetPasswordToken(ProjectUser user)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var encodedEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Email ?? ""));

        return $"/?token={encodedToken}&email={encodedEmail}";
    }
}

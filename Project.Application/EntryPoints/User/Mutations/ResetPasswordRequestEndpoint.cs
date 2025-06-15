using Project.Application.Services;
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
using Project.Domain.Entities;

namespace Project.Application.EntryPoints.User.Mutations;
public sealed class ResetPasswordRequestInputValidator : AbstractValidator<ResetPasswordInput>
{
    public ResetPasswordRequestInputValidator() { }
}
public interface IResetPasswordRequestResolver
{
    Task<MutationResult<object>> ResetPasswordRequestAsync(ResetPasswordRequestInput input, CancellationToken ct);
}
public sealed class ResetPasswordRequestResolver(UserManager<ProjectUser> userManager, IProjectContextFactory projectContextFactory, IMailService mailService, RazorTemplateRenderer razorTemplateRenderer) : IResetPasswordRequestResolver
{
    public async Task<MutationResult<object>> ResetPasswordRequestAsync([UseFluentValidation, UseValidator<ResetPasswordRequestInputValidator>] ResetPasswordRequestInput input, CancellationToken ct)
    {
        using var ctx = projectContextFactory.CreateDbContext();

        var user = await ctx.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == input.Email, ct)
        ?? throw new ArgumentException(ValidationMessages.DefaultAuthenticationError);

        await SendForgotPasswordEmailAsync(user, new ForgotPasswordModel(user.Name));

        return MutationResult<object>.Ok("E-mail de redefinição de senha enviado com sucesso! Verifique sua caixa de entrada ou a pasta de spam.", new object());

    }

    private async Task SendForgotPasswordEmailAsync(ProjectUser user, ForgotPasswordModel model)
    {
        model.Url += await GenerateResetPasswordToken(user);

        var templateHtml = await razorTemplateRenderer.RenderAsync<ForgotPasswordModelTemplate>(model);

        await mailService.SendMailAsync(new MailRequest
        {
            Subject = "PROJECT - Redefinição de Senha",
            ContactName = user.Name,
            ContactMail = user.Email!,
            BodyHtml = templateHtml
        });
    }

    private async Task<string> GenerateResetPasswordToken(ProjectUser user)
    {
        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var encodedEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Email ?? ""));

        return $"/?token={encodedToken}&email={encodedEmail}";
    }
}

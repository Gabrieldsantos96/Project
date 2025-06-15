using AppAny.HotChocolate.FluentValidation;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Application.Services;
using Project.Domain.Entities;
using Project.Domain.Interfaces.Infra;
using Project.Domain.Models;
using Project.Shared.Dtos.User;
using Project.Shared.Validations;

namespace Project.Application.EntryPoints.User.Mutations;
public sealed class SigninInputValidator : AbstractValidator<SigninInput>
{
    public SigninInputValidator()
    {
        RuleFor(s => s.Email)
            .NotEmpty().WithMessage(ValidationMessages.DefaultAuthenticationError);

        RuleFor(s => s.Password)
            .NotEmpty().WithMessage(ValidationMessages.DefaultAuthenticationError);
    }
}
public interface ISigninResolver
{
    Task<MutationResult<AuthenticationDto>> SigninAsync(SigninInput input, CancellationToken ct);
}
public sealed class SigninResolver(SignInManager<ProjectUser> _signInManager, IGraphQL _graphQL,IClaimsService _claimsService, IProjectContextFactory _projectContextFactory, ITokenHandler _tokenHandler) : ISigninResolver
{
    public async Task<MutationResult<AuthenticationDto>> SigninAsync(
        [UseFluentValidation, UseValidator<SigninInputValidator>] SigninInput input,
        CancellationToken ct)
    {

        await using var ctx = await _projectContextFactory.CreateDbContextAsync();

        var user = await _graphQL.ExecuteQueryAsync(
           ctx,
           async context => await context.Users.AsNoTracking()
               .FirstOrDefaultAsync(u => u.Email == input.Email), ct)
           ?? throw new ArgumentException(ValidationMessages.DefaultAuthenticationError);

        var result = await _signInManager.CheckPasswordSignInAsync(user, input.Password, false);

        if (result.Succeeded)
        {
            var current = user.Employees.FirstOrDefault(s => s.Id == user.EmployeeSelectedId);

            var claims = _claimsService.GenerateClaims(user, current);

            var (accessToken, refreshTokenHash) = _tokenHandler.CreateJwt(claims);

            var refreshToken = new RefreshToken()
            {
                UserRefId = user.RefId,
                TokenHash = refreshTokenHash
            };

            ctx.RefreshTokens.Add(refreshToken);

            await ctx.SaveChangesAsync(ct);

            return MutationResult<AuthenticationDto>.Ok("Usuário autenticado com sucesso", new AuthenticationDto(accessToken, refreshTokenHash));
        }

        if (result.IsNotAllowed) throw new Exception(ValidationMessages.IsNotAllowed);

        if (result.IsLockedOut) throw new ArgumentException(ValidationMessages.UserLockedOut);

        throw new ArgumentException(ValidationMessages.DefaultAuthenticationError);
    }
}
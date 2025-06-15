using AppAny.HotChocolate.FluentValidation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project.Application.Services;
using Project.Domain.Entities;
using Project.Domain.Extensions;
using Project.Domain.Interfaces.Infra;
using Project.Domain.Models;
using Project.Shared.Dtos.User;
using Project.Shared.Validations;


namespace Project.Application.EntryPoints.User.Mutations;
public sealed class RefreshTokenInputValidator : AbstractValidator<RefreshTokenInput>
{
    public RefreshTokenInputValidator()
    {
        RuleFor(s => s.RefreshToken)
            .NotEmpty().WithMessage(ValidationMessages.DefaultAuthenticationError)
            .Must(RefreshTokenValidator.IsValidRefreshToken).WithMessage("O refresh token deve ser uma string Base64 válida com 32 bytes codificados (44 caracteres).");
    }
}

public interface IRefreshTokenResolver
{
    Task<MutationResult<AuthenticationDto>> RefreshTokenAsync(RefreshTokenInput input, CancellationToken ct);
}
public sealed class RefreshTokenResolver(ITokenHandler tokenHandler, IClaimsService claimsService, IProjectContextFactory projectContextFactory) : IRefreshTokenResolver
{
    public async Task<MutationResult<AuthenticationDto>> RefreshTokenAsync([UseFluentValidation, UseValidator<RefreshTokenInputValidator>] RefreshTokenInput input, CancellationToken ct)
    {
        using var ctx = projectContextFactory.CreateDbContext();

        var userRefId = claimsService.GetUserRefId();

        var currentRefreshToken = await ctx.RefreshTokens
                .FirstOrDefaultAsync(u => u.TokenHash == input.RefreshToken, ct) ?? throw new NotFoundException();

        if (currentRefreshToken.ExpiresOnUtc < DateTime.Now)
        {
            ctx.RefreshTokens.Remove(currentRefreshToken);
            await ctx.SaveChangesAsync(ct);

            throw new ArgumentException("Refresh token expirado");
        }

        var user = await ctx.Users
                .FirstOrDefaultAsync(u => u.RefId == userRefId, ct) ?? throw new NotFoundException();

        var current = user.Employees.FirstOrDefault(s => s.Id == user.EmployeeSelectedId);

        var claims = claimsService.GenerateClaims(user, current);

        var (accessToken, refreshTokenHash) = tokenHandler.CreateJwt(claims);

        var newRefreshToken = new RefreshToken()
        {
            UserRefId = claimsService.GetUserRefId(),
            TokenHash = refreshTokenHash
        };

        ctx.RefreshTokens.Remove(currentRefreshToken);

        ctx.RefreshTokens.Add(newRefreshToken);

        await ctx.SaveChangesAsync(ct);

        return MutationResult<AuthenticationDto>.Ok("Usuário autenticado com sucesso", new AuthenticationDto(accessToken, refreshTokenHash));
    }
}

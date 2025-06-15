using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Project.Domain.Extensions;
using Project.Domain.Interfaces.Infra;
using Project.Domain.Models;
using Project.Shared.Dtos.User;

namespace Project.Application.EntryPoints.User.Mutations;
public interface ISignoutResolver
{
    Task<MutationResult<object>> SignOutAsync(RefreshTokenInput input, CancellationToken ct);
}
public sealed class SignoutResolver(IProjectContextFactory projectContextFactory, IClaimsService claimsService) : ISignoutResolver
{
    public async Task<MutationResult<object>> SignOutAsync(RefreshTokenInput input, CancellationToken ct)
    {
        await using var ctx = await projectContextFactory.CreateDbContextAsync();

        var refreshToken = await ctx.RefreshTokens
       .FirstOrDefaultAsync(u => u.TokenHash == input.RefreshToken && u.UserRefId == claimsService.GetUserRefId(), ct) ?? throw new NotFoundException();

        ctx.RefreshTokens.Remove(refreshToken);

        await ctx.SaveChangesAsync(ct);

        return MutationResult<object>.Ok("Ok", new object());

    }
}

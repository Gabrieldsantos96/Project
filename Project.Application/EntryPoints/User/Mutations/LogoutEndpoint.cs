using Project.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Project.Application.EntryPoints.User.Mutations;

public interface ILogoutResolver
{
    Task<MutationResult> LogoutAsync(CancellationToken ct);
}

public sealed class LogoutResolver(IHttpContextAccessor httpContextAccessor) : ILogoutResolver
{
    public async Task<MutationResult> LogoutAsync(CancellationToken ct)
    {
        if (httpContextAccessor.HttpContext != null)
        {
            await httpContextAccessor.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            return MutationResult.Ok("Logout realizado com sucesso.");
        }

        return default!;
    }
}
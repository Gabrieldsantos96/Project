using Project.Shared.Dtos.User;
using Project.Shared.Models;
using HotChocolate.Authorization;
using Project.Application.EntryPoints.User.Mutations;

namespace Project.Application.Graphql;
public class Mutations
{
    #region User entrypoints

    public async Task<MutationResult> AuthenticateUserAsync(
        [Service] IAuthenticateUserResolver authResolver,
        UserAuthenticationInput input,
        CancellationToken ct)
    {
        return await authResolver.AuthenticateUserAsync(input, ct);
    }

    [Authorize]
    public async Task<MutationResult> LogoutAsync(
        [Service] ILogoutResolver logoutResolver,
        CancellationToken ct)
    {
        return await logoutResolver.LogoutAsync(ct);
    }

    public async Task<UserProfileDto> CreateAccountAsync(
        [Service] ICreateAccountResolver createAccountResolver,
        CreateAccountInput input,
        CancellationToken ct)
    {
        return await createAccountResolver.CreateAccountAsync(input, ct);
    }

    [Authorize]
    public async Task<MutationResult> ChangeJobRoleAsync(
        [Service] IChangeJobRoleResolver changeJobRoleResolver,
        ChangeJobRoleInput input,
        CancellationToken ct)
    {
        return await changeJobRoleResolver.ChangeJobRoleAsync(input, ct);
    }

    [Authorize]
    public async Task<MutationResult> ChangePasswordAsync(
        [Service] IUserChangePasswordResolver userChangePasswordResolver,
        ChangePasswordInput input,
        CancellationToken ct)
    {
        return await userChangePasswordResolver.ChangePasswordAsync(input, ct);
    }

    public async Task<MutationResult> ResetPasswordRequestAsync(
        [Service] IResetPasswordRequestResolver resetPasswordRequestResolver,
        ResetPasswordRequestInput input,
        CancellationToken ct)
    {
        return await resetPasswordRequestResolver.ResetPasswordRequestAsync(input, ct);
    }

    public async Task<MutationResult> ResetPasswordAsync(
        [Service] IResetPasswordResolver resetPasswordResolver,
        ResetPasswordInput input,
        CancellationToken ct)
    {
        return await resetPasswordResolver.ResetPasswordAsync(input, ct);
    }

    #endregion
}
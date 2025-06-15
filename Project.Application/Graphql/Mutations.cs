using HotChocolate.Authorization;
using Project.Application.EntryPoints.User.Mutations;
using Project.Application.EntryPoints.Workflow.Mutations;
using Project.Domain.Entities;
using Project.Domain.Models;
using Project.Shared.Dtos.User;
using Project.Shared.Dtos.Workflow;

namespace Project.Application.Graphql;
public class Mutations
{
    #region User entrypoints
    public async Task<MutationResult<AuthenticationDto>> SigninAsync(
        [Service] ISigninResolver signinResolver,
        SigninInput input,
        CancellationToken ct)
    {
        return await signinResolver.SigninAsync(input, ct);
    }

    [Authorize]
    public async Task<MutationResult<object>> SignoutAsync(
        [Service] ISignoutResolver signoutResolver,
        RefreshTokenInput input,
        CancellationToken ct)
    {
        return await signoutResolver.SignOutAsync(input, ct);
    }

    [Authorize]
    public async Task<MutationResult<AuthenticationDto>> RefreshTokenAsync(
      [Service] IRefreshTokenResolver refreshTokenResolver,
      RefreshTokenInput input,
      CancellationToken ct)
    {
        return await refreshTokenResolver.RefreshTokenAsync(input, ct);
    }

    public async Task<UserProfileDto> CreateAccountAsync(
        [Service] ICreateAccountResolver createAccountResolver,
        CreateAccountInput input,
        CancellationToken ct)
    {
        return await createAccountResolver.CreateAccountAsync(input, ct);
    }

    [Authorize]
    public async Task<MutationResult<object>> ChangeJobRoleAsync(
        [Service] IChangeJobRoleResolver changeJobRoleResolver,
        ChangeJobRoleInput input,
        CancellationToken ct)
    {
        return await changeJobRoleResolver.ChangeJobRoleAsync(input, ct);
    }

    [Authorize]
    public async Task<MutationResult<object>> ChangePasswordAsync(
        [Service] IUserChangePasswordResolver userChangePasswordResolver,
        ChangePasswordInput input,
        CancellationToken ct)
    {
        return await userChangePasswordResolver.ChangePasswordAsync(input, ct);
    }

    public async Task<MutationResult<object>> ResetPasswordRequestAsync(
        [Service] IResetPasswordRequestResolver resetPasswordRequestResolver,
        ResetPasswordRequestInput input,
        CancellationToken ct)
    {
        return await resetPasswordRequestResolver.ResetPasswordRequestAsync(input, ct);
    }

    [Authorize]
    public async Task<MutationResult<object>> ResetPasswordAsync(
        [Service] IResetPasswordResolver resetPasswordResolver,
        ResetPasswordInput input,
        CancellationToken ct)
    {
        return await resetPasswordResolver.ResetPasswordAsync(input, ct);
    }

    #endregion

    #region Workflow
    public async Task<MutationResult<WorkflowMatching>> CreateWorkflowMatchingAsync(
        [Service] IMatchingResolver matchingResolver,
        CreateWorkflowMatchingInput input,
        CancellationToken ct)
    {
        return await matchingResolver.CreateMatchingAsync(input, ct);
    }
    public async Task<MutationResult<WorkflowPurchaseOrder>> CreateWorkflowPurchaseOrderAsync(
        [Service] IPurchaseOrderResolver purchaseOrderResolver,
        CreateWorkflowPurchaseOrderInput input,
        CancellationToken ct)
    {
        return await purchaseOrderResolver.CreatePurchaseOrderAsync(input, ct);
    }

    #endregion
}
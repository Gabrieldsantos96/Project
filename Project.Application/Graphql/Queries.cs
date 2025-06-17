using Project.Application.EntryPoints.Workflow.Queries;
using Project.Application.Features.User.Queries;
using Project.Domain.Entities;
using Project.Domain.Models;
using Project.Shared.Dtos.User;
using Raven.Client.Documents.Linq;


namespace Project.Application.Graphql;
public class Queries()
{
    public Task<UserProfileDto> GetUserProfileAsync([Service] IGetUserProfileResolver _getUserProfileResolver, CancellationToken ct) => _getUserProfileResolver.GetUserProfileAsync(ct);
    public Task<MutationResult<WorkflowBase>> FindWorkflow([Service] IFindWorkflowResolver _findWorkflowResolver, Guid refId, CancellationToken ct) => _findWorkflowResolver.FindWorkflow(refId, ct);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IRavenQueryable<WorkflowBase> ListBases([Service] IListWorkflowResolvers _listWorkflowsResolvers, CancellationToken ct) => _listWorkflowsResolvers.ListBases(ct);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IRavenQueryable<WorkflowMatching> ListMatchings([Service] IListWorkflowResolvers _listWorkflowsResolvers, CancellationToken ct) => _listWorkflowsResolvers.ListMatchings(ct);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IRavenQueryable<WorkflowPurchaseOrder> ListPurchaseOrders([Service] IListWorkflowResolvers _listWorkflowsResolvers, CancellationToken ct) => _listWorkflowsResolvers.ListPurchaseOrders(ct);
}

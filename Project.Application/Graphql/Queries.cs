using Project.Application.EntryPoints.Workflow.Queries;
using Project.Application.Features.User.Queries;
using Project.Application.Graphql.Schema;
using Project.Domain.Entities;
using Project.Domain.Models;
using Project.Shared.Dtos.User;
using Raven.Client.Documents.Linq;

namespace Project.Application.Graphql;
public class Queries()
{
    public Task<UserProfileDto> GetUserProfileAsync([Service] IGetUserProfileResolver _getUserProfileResolver, CancellationToken ct) => _getUserProfileResolver.GetUserProfileAsync(ct);
    public Task<MutationResult<WorkflowBase>> FindWorkflowByRef([Service] IFindWorkflowByRefResolver _findWorkflowByRefResolver, Guid refId, CancellationToken ct) => _findWorkflowByRefResolver.FindWorkflowByRef(refId, ct);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IRavenQueryable<IWorkflow> ListWorkflows([Service] IListWorkflowsResolver _listWorkflowsResolver, CancellationToken ct) => _listWorkflowsResolver.ListWorkflows(ct);
}

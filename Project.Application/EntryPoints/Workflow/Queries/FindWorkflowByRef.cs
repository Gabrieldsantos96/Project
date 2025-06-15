using Microsoft.EntityFrameworkCore;
using Project.Domain.Entities;
using Project.Domain.Interfaces.Infra;
using Project.Domain.Models;
using Project.Shared.Validations;

namespace Project.Application.EntryPoints.Workflow.Queries;
public interface IFindWorkflowByRefResolver
{
    Task<MutationResult<WorkflowBase>> FindWorkflowByRef(Guid refId, CancellationToken ct);
}
public sealed class FindWorkflowByRefResolver(IProjectContextFactory _projectContextFactory) : IFindWorkflowByRefResolver
{
    public async Task<MutationResult<WorkflowBase>> FindWorkflowByRef(
        Guid refId, CancellationToken ct)
    {
        using var ctx = _projectContextFactory.CreateDbContext();

        var result = await ctx.Workflows.FirstOrDefaultAsync(s => s.RefId == refId, cancellationToken: ct) ?? throw new ArgumentException(ValidationMessages.NotFound);

        return MutationResult<WorkflowBase>.Ok("Ok", result);
    }
}
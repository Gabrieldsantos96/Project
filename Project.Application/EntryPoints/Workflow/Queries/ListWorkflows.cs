using Project.Domain.Entities;
using Project.Domain.Interfaces.Infra;

namespace Project.Application.EntryPoints.Workflow.Queries;
public interface IListWorkflowsResolver
{
    IQueryable<WorkflowBase> ListWorkflows(CancellationToken ct);
}
public sealed class ListWorkflowsResolver(IProjectContextFactory projectContextFactory) : IListWorkflowsResolver
{
    public IQueryable<WorkflowBase> ListWorkflows(CancellationToken ct)
    {
        using var ctx = projectContextFactory.CreateDbContext();

        return ctx.Workflows.AsQueryable();

    }
}
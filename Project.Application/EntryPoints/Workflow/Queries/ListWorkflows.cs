using Project.Application.Graphql.Schema;
using Project.Domain.Interfaces.Infra;
using Raven.Client.Documents.Linq;

namespace Project.Application.EntryPoints.Workflow.Queries;
public interface IListWorkflowsResolver
{
    IRavenQueryable<IWorkflow> ListWorkflows(CancellationToken ct);
}
public sealed class ListWorkflowsResolver(IRavenSessionFactory sessionFactory) : IListWorkflowsResolver
{
    public IRavenQueryable<IWorkflow> ListWorkflows(CancellationToken ct)
    {
        using var session = sessionFactory.OpenSession();

        return session.Query<IWorkflow>();
    }
}
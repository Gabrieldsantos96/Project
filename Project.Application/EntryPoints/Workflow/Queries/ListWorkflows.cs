using Project.Domain.Entities;
using Project.Domain.Interfaces.Infra;
using Raven.Client.Documents.Linq;

namespace Project.Application.EntryPoints.Workflow.Queries;
public interface IListWorkflowResolvers
{
    IRavenQueryable<WorkflowMatching> ListMatchings(CancellationToken ct);

    IRavenQueryable<WorkflowPurchaseOrder> ListPurchaseOrders(CancellationToken ct);

    IRavenQueryable<WorkflowBase> ListBases(CancellationToken ct);
}
public sealed class ListWorkflowsResolvers(IRavenSessionFactory sessionFactory) : IListWorkflowResolvers
{
    public IRavenQueryable<WorkflowMatching> ListMatchings(CancellationToken ct)
    {
        var session = sessionFactory.OpenSessionAsync();
        return session.Query<WorkflowMatching>();
    }
    public IRavenQueryable<WorkflowPurchaseOrder> ListPurchaseOrders(CancellationToken ct)
    {
        var session = sessionFactory.OpenSessionAsync();
        return session.Query<WorkflowPurchaseOrder>();
    }
    public IRavenQueryable<WorkflowBase> ListBases(CancellationToken ct)
    {
        var session = sessionFactory.OpenSessionAsync();
        return session.Query<WorkflowBase>();
    }
}
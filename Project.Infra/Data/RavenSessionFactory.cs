using Project.Domain.Interfaces.Infra;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Project.Infra.Data;
public class RavenSessionFactory : IRavenSessionFactory
{
    private readonly IDocumentStore _documentStore;

    public RavenSessionFactory(IDocumentStore documentStore)
    {
        _documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
    }

    public IDocumentSession OpenSession()
    {
        return _documentStore.OpenSession();
    }

    public IAsyncDocumentSession OpenSessionAsync()
    {
        return _documentStore.OpenAsyncSession();
    }
}
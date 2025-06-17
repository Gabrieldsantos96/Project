using Raven.Client.Documents.Session;

namespace Project.Domain.Interfaces.Infra;
public interface IRavenSessionFactory
{
    IDocumentSession OpenSession();
    IAsyncDocumentSession OpenSessionAsync();
}
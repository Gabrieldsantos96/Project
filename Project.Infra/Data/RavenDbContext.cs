using System.Security.Cryptography.X509Certificates;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Project.Infra.Data;
public static class RavenDbContext
{
    public static IDocumentStore CreateDocumentStore(string databaseName, string[] urls, X509Certificate2 certificate)
    {
        if (certificate is null)
            throw new InvalidOperationException();

        var store = new DocumentStore
        {
            Urls = urls,
            Database = databaseName,
            Certificate = certificate,
            Conventions =
            {
                DisposeCertificate = false,
                UseOptimisticConcurrency  = true,
            }
        };

        try
        {
            var createDbOp = new CreateDatabaseOperation(new DatabaseRecord(databaseName));
            store.Maintenance.Server.Send(createDbOp);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Aviso ao criar database '{databaseName}': {ex.Message}");
        }

        Console.WriteLine($"Conectado ao database:'{databaseName}");

        store.Initialize();

        return store;
    }

    public static IDocumentSession OpenSession(this IDocumentStore store) =>
        store.OpenSession();
}

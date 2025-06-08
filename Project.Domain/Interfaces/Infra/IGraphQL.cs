namespace Project.Domain.Interfaces.Infra;
public interface IGraphQL
{
    Task<T> ExecuteQueryAsync<T>(
        IProjectContext ctx,
        Func<IProjectContext, Task<T>> query,
        CancellationToken cancellationToken = default);

    Task<T> ExecuteQueryAsync<T>(
        Func<IProjectContext, Task<T>> query,
        CancellationToken cancellationToken = default);

    IQueryable<T> ExecuteQueryableAsync<T>(
            IProjectContext ctx,
            Func<IProjectContext, IQueryable<T>> query,
            CancellationToken cancellationToken = default);

    Task<IQueryable<T>> ExecuteQueryableAsync<T>(
        Func<IProjectContext, IQueryable<T>> query,
        CancellationToken cancellationToken = default);

    Task<List<T>> ExecuteListAsync<T>(
        IProjectContext ctx,
        Func<IProjectContext, IQueryable<T>> query,
        CancellationToken cancellationToken = default);
    Task<List<T>> ExecuteListAsync<T>(
      Func<IProjectContext, IQueryable<T>> query,
      CancellationToken cancellationToken = default);
    Task<T> ExecuteMutationAsync<T>(
        IProjectContext ctx,
        Func<IProjectContext, Task<T>> mutation,
        CancellationToken cancellationToken = default);
    Task<T> ExecuteMutationAsync<T>(
      Func<IProjectContext, Task<T>> mutation,
      CancellationToken cancellationToken = default);

    Task<T> ExecuteTransactionAsync<T>(
        IProjectContext ctx,
        Func<IProjectContext, Task<T>> transactionOperation,
        CancellationToken cancellationToken = default);

    Task<T> ExecuteTransactionAsync<T>(
       Func<IProjectContext, Task<T>> transactionOperation,
       CancellationToken cancellationToken = default);
}
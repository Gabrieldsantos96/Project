using Microsoft.EntityFrameworkCore;
using Project.Domain.Interfaces.Infra;

namespace Project.Application.Graphql;
public sealed class Executor(IProjectContextFactory _projectContextFactory) : IGraphQL
{

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<T> ExecuteQueryAsync<T>(
        IProjectContext ctx,
        Func<IProjectContext, Task<T>> query,
        CancellationToken cancellationToken = default)
    {
        return await query(ctx);
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<T> ExecuteQueryAsync<T>(
        Func<IProjectContext, Task<T>> query,
        CancellationToken cancellationToken = default)
    {
        await using var ctx = await _projectContextFactory.CreateDbContextAsync();
        return await query(ctx);
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<T> ExecuteQueryableAsync<T>(
        IProjectContext ctx,
        Func<IProjectContext, IQueryable<T>> query,
        CancellationToken cancellationToken = default)
    {
        return query(ctx);
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<T>> ExecuteQueryableAsync<T>(
        Func<IProjectContext, IQueryable<T>> query,
        CancellationToken cancellationToken = default)
    {
        await using var ctx = await _projectContextFactory.CreateDbContextAsync();
        return query(ctx);
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<List<T>> ExecuteListAsync<T>(
        IProjectContext ctx,
        Func<IProjectContext, IQueryable<T>> query,
        CancellationToken cancellationToken = default)
    {
        return await query(ctx).ToListAsync(cancellationToken);
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<List<T>> ExecuteListAsync<T>(
        Func<IProjectContext, IQueryable<T>> query,
        CancellationToken cancellationToken = default)
    {
        await using var ctx = await _projectContextFactory.CreateDbContextAsync();
        return await query(ctx).ToListAsync(cancellationToken);
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<T> ExecuteMutationAsync<T>(
        IProjectContext ctx,
        Func<IProjectContext, Task<T>> mutation,
        CancellationToken cancellationToken = default)
    {
        var result = await mutation(ctx);
        await ctx.SaveChangesAsync(cancellationToken);
        return result;
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<T> ExecuteMutationAsync<T>(
        Func<IProjectContext, Task<T>> mutation,
        CancellationToken cancellationToken = default)
    {
        await using var ctx = await _projectContextFactory.CreateDbContextAsync();
        var result = await mutation(ctx);
        await ctx.SaveChangesAsync(cancellationToken);
        return result;
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<T> ExecuteTransactionAsync<T>(
        IProjectContext ctx,
        Func<IProjectContext, Task<T>> transactionOperation,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await ctx.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await transactionOperation(ctx);
            await ctx.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<T> ExecuteTransactionAsync<T>(
        Func<IProjectContext, Task<T>> transactionOperation,
        CancellationToken cancellationToken = default)
    {
        await using var ctx = await _projectContextFactory.CreateDbContextAsync();
        await using var transaction = await ctx.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await transactionOperation(ctx);
            await ctx.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
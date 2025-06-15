using FluentValidation;
using Project.Domain.Entities;
using Project.Domain.Models;
using Project.Shared.Dtos.Workflow;

namespace Project.Application.EntryPoints.Workflow.Mutations;
public sealed class CreatePurchaseOrderInputValidator : AbstractValidator<CreateWorkflowPurchaseOrderInput>
{
    public CreatePurchaseOrderInputValidator()
    {
    }
}
public interface IPurchaseOrderResolver
{
    Task<MutationResult<WorkflowPurchaseOrder>> CreatePurchaseOrderAsync(CreateWorkflowPurchaseOrderInput input, CancellationToken ct);
}
public sealed class CreatePurchaseOrderResolver : IPurchaseOrderResolver
{
    public async Task<MutationResult<WorkflowPurchaseOrder>> CreatePurchaseOrderAsync(
        CreateWorkflowPurchaseOrderInput input, CancellationToken ct)
    {
        await Task.Delay(1000);

        var workflow = new WorkflowPurchaseOrder();

        return MutationResult<WorkflowPurchaseOrder>.Ok("", workflow);
    }
}
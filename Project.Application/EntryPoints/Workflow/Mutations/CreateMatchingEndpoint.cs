using AppAny.HotChocolate.FluentValidation;
using FluentValidation;
using Project.Domain.Entities;
using Project.Domain.Models;
using Project.Shared.Dtos.Workflow;

namespace Project.Application.EntryPoints.Workflow.Mutations;
public sealed class CreateMatchingInputValidator : AbstractValidator<CreateWorkflowMatchingInput>
{
    public CreateMatchingInputValidator()
    {
    }
}
public interface IMatchingResolver
{
    Task<MutationResult<WorkflowMatching>> CreateMatchingAsync(CreateWorkflowMatchingInput input, CancellationToken ct);
}
public sealed class CreateMatchingResolver : IMatchingResolver
{
    public async Task<MutationResult<WorkflowMatching>> CreateMatchingAsync(
        [UseFluentValidation, UseValidator<CreateMatchingInputValidator>] CreateWorkflowMatchingInput input, CancellationToken ct)
    {
        await Task.Delay(1000);

        var workflow = new WorkflowMatching();

        return MutationResult<WorkflowMatching>.Ok("", workflow);
    }
}
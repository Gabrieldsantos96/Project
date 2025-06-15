using System.Text.Json.Serialization;

namespace Project.Shared.Dtos.Workflow;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CreateWorkflowPurchaseOrderInput), typeDiscriminator: nameof(CreateWorkflowPurchaseOrderInput))]
[JsonDerivedType(typeof(CreateWorkflowMatchingInput), typeDiscriminator: nameof(CreateWorkflowMatchingInput))]
public class CreateWorkflowBaseInput
{
    public string Type { get; set; } = null!;
}
public class CreateWorkflowPurchaseOrderInput : CreateWorkflowBaseInput
{
    public decimal Amount { get; set; }
}
public class CreateWorkflowMatchingInput : CreateWorkflowBaseInput
{
    public string MatchCriteria { get; set; } = string.Empty;
}
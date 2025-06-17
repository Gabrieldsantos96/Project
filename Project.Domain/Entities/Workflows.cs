using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Project.Domain.Entities;

public interface IWorkflow : ITenant
{
    int Id { get; }
    Guid RefId { get; }
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(WorkflowPurchaseOrder), typeDiscriminator: nameof(WorkflowPurchaseOrder))]
[JsonDerivedType(typeof(WorkflowMatching), typeDiscriminator: nameof(WorkflowMatching))]
public class WorkflowBase : Entity, ITenant, IWorkflow
{
    public Guid RefId { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(TenantId))]
    public int TenantId { get; set; }
    [JsonIgnore]
    public Tenant Tenant { get; set; } = null!;
}
public class WorkflowPurchaseOrder : WorkflowBase
{
    public decimal Amount { get; set; }
}
public class WorkflowMatching : WorkflowBase
{
    public string MatchCriteria { get; set; } = null!;
}
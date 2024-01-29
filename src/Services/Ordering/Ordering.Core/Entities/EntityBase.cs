namespace Ordering.Core.Entities;

public abstract class EntityBase
{
    public int Id { get; protected set; }
    //Audit Properties
    public string? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
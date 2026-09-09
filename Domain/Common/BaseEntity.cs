namespace Domain.Common;

public class BaseEntity
{
    public Guid PublicId { get; protected set; } = Guid.NewGuid();
    public int Id { get; private set; }
}

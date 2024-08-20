namespace KarnelTravel.Domain.Common;

public abstract class BaseEntity :  DomainEventEntity
{
	public int Id { get; set; }
}

public abstract class BaseEntity<T> : DomainEventEntity
{
	public virtual T Id { get; protected set; }

	protected BaseEntity() 
	{
	}

	protected BaseEntity(T id)
	{
		Id = id;
	}
}


namespace KarnelTravel.Domain.Common;

public class BaseAuditableEntity : BaseEntity, IAuditableEntity
{
	public DateTimeOffset Created { get; set; }

	public string CreatedBy { get; set; }

	public DateTimeOffset LastModified { get; set; }

	public string LastModifiedBy { get; set; }

	public bool IsDeleted { get; set; } = false;
}

public abstract class BaseAuditableEntity<T> : BaseEntity<T>, IAuditableEntity
{
	public DateTimeOffset Created { get; set; }

	public string CreatedBy { get; set; }

	public DateTimeOffset LastModified { get; set; }

	public string LastModifiedBy { get; set; }

	public bool IsDeleted { get; set; } = false;
}


public abstract class BaseAuditableAuditLogEntity : BaseEntity<long>, IAuditableEntity
{
	public string DisplayName { get; set; }

	public string FieldName { get; set; }

	public string OldValue { get; set; }

	public string NewValue { get; set; }

	public DateTimeOffset Created { get; set; }

	public string CreatedBy { get; set; }

	public DateTimeOffset LastModified { get; set; }

	public string LastModifiedBy { get; set; }
}
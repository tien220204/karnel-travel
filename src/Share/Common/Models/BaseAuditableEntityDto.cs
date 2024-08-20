namespace Share.Common.Models;
public class BaseAuditableEntityDto
{
    public int Id { get; set; }

    public DateTimeOffset Created { get; set; }

    public string CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    public string LastModifiedBy { get; set; }
}

public class BaseAuditableEntityDto<T>
{
    public T Id { get; set; }

    public DateTimeOffset Created { get; set; }

    public string CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    public string LastModifiedBy { get; set; }
}

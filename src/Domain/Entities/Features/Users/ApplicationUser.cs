using KarnelTravel.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace KarnelTravel.Domain.Entities.Features.Users;

public class ApplicationUser : IdentityUser, IAuditableEntity
{
    public DateTime? DateOfBirth { get; set; }

    public string FullName { get; set; } = null!;

    public int? ProvinceId { get; set; }

    public string ProvinceName { get; set; }

    public int? DistrictId { get; set; }

    public string DistrictName { get; set; }

    public int? WardId { get; set; }

    public string WardName { get; set; }

    public string Address { get; set; }

    public DateTimeOffset Created { get; set; }

    public string CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    public string LastModifiedBy { get; set; }

    public virtual ICollection<ApplicationUserClaim> Claims { get; set; } = new List<ApplicationUserClaim>();

    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
}

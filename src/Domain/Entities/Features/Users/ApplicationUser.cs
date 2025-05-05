using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Entities.Features.Hotels;
using Microsoft.AspNetCore.Identity;

namespace KarnelTravel.Domain.Entities.Features.Users;

public class ApplicationUser : BaseAuditableEntity<Guid>
{
	public string KeycloakId { get; set; } = null!;

	public DateTime? DateOfBirth { get; set; }

    public string FullName { get; set; } = null!;

    public int? ProvinceId { get; set; }

    public string ProvinceName { get; set; }

    public int? DistrictId { get; set; }

    public string DistrictName { get; set; }

    public int? WardId { get; set; }

    public string WardName { get; set; }

    public string Address { get; set; }

	public string Email { get; set; }
}

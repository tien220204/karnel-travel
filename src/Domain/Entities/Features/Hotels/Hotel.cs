using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Enums;

namespace KarnelTravel.Domain.Entities.Features.Hotels;
public class Hotel : BaseAuditableEntity<Guid>
{
	public string Name { get; set; }
	public PropertyType PropertyType { get; set; }
	public HotelClass HotelClass { get; set; }
	public ICollection<HotelAmenity> ProductAmenities { get; set; }
}

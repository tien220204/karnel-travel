using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Entities.Features.Hotels;

namespace KarnelTravel.Domain.Entities.Features.Hotel;
public class Style : BaseAuditableEntity<long>
{
	public string Name { get; set; }
	public string Description { get; set; }
	public ICollection<HotelStyle> HotelStyles { get; set; }
}

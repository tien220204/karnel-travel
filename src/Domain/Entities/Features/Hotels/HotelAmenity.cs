using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Entities.Features.MasterData;

namespace KarnelTravel.Domain.Entities.Features.Hotels;
public class HotelAmenity : BaseAuditableEntity<long>
{
    public long HotelId { get; set; }
	public long AmenityId { get; set; }
	public Hotel Hotel { get; set; }
	public Amenity Amenity { get; set; }
}

using KarnelTravel.Domain.Common;

namespace KarnelTravel.Domain.Entities.Features.Hotels;
public class HotelAmenity : BaseAuditableEntity<long>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public long HotelId { get; set; }
    public Hotel Hotel { get; set; }

}

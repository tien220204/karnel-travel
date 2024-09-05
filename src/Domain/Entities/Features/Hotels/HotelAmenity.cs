using KarnelTravel.Domain.Common;

namespace KarnelTravel.Domain.Entities.Features.Hotels;
public class HotelAmenity : BaseAuditableEntity<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
}

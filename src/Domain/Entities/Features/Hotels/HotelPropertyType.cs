using KarnelTravel.Domain.Common;

namespace KarnelTravel.Domain.Entities.Features.Hotel;
public class HotelPropertyType : BaseAuditableEntity<long>
{
    public string Name { get; set; }
    public string Description { get; set; }
}

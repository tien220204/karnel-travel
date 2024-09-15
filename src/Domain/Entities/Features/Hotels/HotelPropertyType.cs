using KarnelTravel.Domain.Common;

namespace KarnelTravel.Domain.Entities.Features.Hotels;
public class HotelPropertyType : BaseAuditableEntity<long>
{
    public string Name { get; set; }
    public string Description { get; set; }
    
}

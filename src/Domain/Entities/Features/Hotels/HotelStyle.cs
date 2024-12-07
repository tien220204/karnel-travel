using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Entities.Features.Hotel;

namespace KarnelTravel.Domain.Entities.Features.Hotels;
public class HotelStyle : BaseAuditableEntity<long>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public long HotelId { get; set; }
    public long StyleId { get; set; }
    public Style Style { get; set; }
    public Hotel Hotel { get; set; }
}

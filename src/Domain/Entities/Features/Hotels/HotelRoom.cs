using KarnelTravel.Domain.Common;

namespace KarnelTravel.Domain.Entities.Features.Hotels;
public class HotelRoom : BaseAuditableEntity<Guid>
{
    public string Code { get; set; }
    public Guid HotelId { get; set; }
    public Hotel Hotel { get; set; }
}

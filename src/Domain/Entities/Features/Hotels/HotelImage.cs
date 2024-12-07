using KarnelTravel.Domain.Common;

namespace KarnelTravel.Domain.Entities.Features.Hotels;
public class HotelImage : BaseAuditableEntity<long>
{
    public string Name { get; set; }
    public string Url { get; set; }
    public bool IsAvatar { get; set; }
    public long HotelId { get; set; }
    public Hotel Hotel { get; set; }
}

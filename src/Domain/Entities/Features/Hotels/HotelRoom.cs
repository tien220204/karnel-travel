
using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Enums.Hotels;

namespace KarnelTravel.Domain.Entities.Features.Hotels;
public class HotelRoom : BaseAuditableEntity<long>
{
    public string Code { get; set; }
	public string Description { get; set; }
    public long Capacity { get; set; }
	public long PricePerHour { get; set; }
	public long HotelId { get; set; }
    public Hotel Hotel { get; set; }
}

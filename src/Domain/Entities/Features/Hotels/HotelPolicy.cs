using KarnelTravel.Domain.Common;

namespace KarnelTravel.Domain.Entities.Features.Hotels;
public class HotelPolicy : BaseAuditableEntity<long>
{
	public string Type { get; set; }
	public string Description { get; set; }
	public Guid HotelId { get; set; }
    public Hotel Hotel { get; set; }
}

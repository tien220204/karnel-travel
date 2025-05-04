using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Entities.Features.Users;

namespace KarnelTravel.Domain.Entities.Features.Hotels;


public class HotelRating : BaseAuditableEntity<long>
{
	public decimal StarRate { get; set; }
	public Guid UserId { get; set; }
	public long HotelId { get; set; }
	public ApplicationUser User { get; set; }
	public Hotel Hotel { get; set; }
}

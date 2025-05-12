using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Entities.Features.Users;
using KarnelTravel.Domain.Enums.Flights;

namespace KarnelTravel.Domain.Entities.Features.Flights;
public class FlightTicket : BaseAuditableEntity<long>
{
    public TicketType TicketType { get; set; }
    public long FLightId { get; set; }
    public Flight Flight { get; set; }
	public string SeatCode { get; set; }
	public Guid UserId { get; set; }
	public TicketStatus TicketStatus { get; set; }
    public ApplicationUser User { get; set; }
}

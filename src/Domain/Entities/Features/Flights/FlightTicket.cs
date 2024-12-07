using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Enums.Flights;

namespace KarnelTravel.Domain.Entities.Features.Flights;
public class FlightTicket : BaseAuditableEntity<long>
{
    public TicketType TicketType { get; set; }
    public long FLightId { get; set; }
    public Flight Flight { get; set; }
    public TicketStatus TicketStatus { get; set; }
}

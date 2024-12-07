using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Entities.Features.Flights;

public class FlightTicketBooking : BaseAuditableEntity<long>
{
	public long FlightTicketPrice { get; set; }
	public long FLightTicketId { get; set; }
	public long BookingDetailId { get; set; }
	public FlightTicket FlightTicket { get; set; }
	public BookingDetail BookingDetail { get; set; }
}

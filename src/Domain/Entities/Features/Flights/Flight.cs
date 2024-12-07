using KarnelTravel.Domain.Common;

namespace KarnelTravel.Domain.Entities.Features.Flights;
public class Flight : BaseAuditableEntity<long>
{
	public string Name { get; set; }
	public string FlightCode { get; set; }
	public long DepartureAirportId { get; set; }
	public long ArrivalAirportId { get; set; }
	public long AirlineId { get; set; }
    public ICollection<FlightTicket> FlightTickets { get; set; }
    public Airline Airline { get; set; }
	public Airport Airport { get; set; }
    public ICollection<FlightExtension> FlightExtensions { get; set; }
}

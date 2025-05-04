using KarnelTravel.Domain.Entities.Features.Flights;
using Share.Common.Models;

namespace KarnelTravel.Application.Features.Flights.Models.Dtos;
public class FlightDto : BaseAuditableEntityDto<long>
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

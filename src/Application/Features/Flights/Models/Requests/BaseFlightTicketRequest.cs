using KarnelTravel.Domain.Enums.Flights;

namespace KarnelTravel.Application.Features.Flights.Models.Requests;
public class BaseFlightTicketRequest
{
	public TicketType TicketType { get; set; }
	public long FLightId { get; set; }
	public string SeatCode { get; set; }
	public TicketStatus TicketStatus { get; set; }
}

public class CreateFlightTicketRequest : BaseFlightTicketRequest
{
	public int Quantity { get; set; }
}

public class UpdateFlightTicketRequest : BaseFlightTicketRequest
{
	public long FlightTicketId { get; set; }
}

public class DeleteFlightTicketRequest
{
	public long FlightTicketId { get; set; }
}
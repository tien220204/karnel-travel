using KarnelTravel.Domain.Entities.Features.Flights;
using KarnelTravel.Share.GoogleSearchApiService.SerpApi;

namespace KarnelTravel.Application.Features.Flights.Models.Requests;
public class BaseFlightRequest
{
	public string Name { get; set; }
	public string FlightCode { get; set; }
	public long DepartureAirportId { get; set; }
	public long ArrivalAirportId { get; set; }
	public long AirlineId { get; set; }
	public Airline Airline { get; set; }
	public Airport Airport { get; set; }
}	

public class CreateFlightRequest : BaseFlightRequest
{
}

public class UpdateFlightRequest : BaseFlightRequest
{
	public long Id { get; set; }
}

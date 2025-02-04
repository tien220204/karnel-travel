using System.Collections;

namespace KarnelTravel.Share.GoogleSearchApiService.SerpApi;

public class FlightSearchApi : SerpApiSearch
{
	public FlightSearchApi(Hashtable parameter, String apiKey) : base(parameter, apiKey, SerpApiSearch.FLIGHT_ENGINE) { }
}
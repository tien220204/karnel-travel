using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Entities.Features.MasterData;

namespace KarnelTravel.Domain.Entities.Features.Flights;
public class Airport : BaseAuditableEntity<long>
{
	public string Name { get; set; }
	public string AirportCode { get; set; }
	public string Timezone { get; set; }
	public string CountryCode { get; set; }
	public string ProvinceCode { get; set; }
	public ICollection<Flight> Flights { get; set; }
	public Country Country { get; set; }
	public Province Province { get; set; }
}

using KarnelTravel.Domain.Entities.Features.Flights;
using KarnelTravel.Domain.Entities.Features.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.Flights.Models.Requests;
public class BaseAirportRequest
{
	public string Name { get; set; }
	public string AirportCode { get; set; }
	public string Timezone { get; set; }
	public string CountryCode { get; set; }
	public string ProvinceCode { get; set; }
}

public class CreateAirportRequest : BaseAirportRequest
{
}

public class UpdateAirportRequest : BaseAirportRequest
{
	public long Id { get; set; }
}

public class DeleteAirportRequest
{
	public long Id { get; set; }
}

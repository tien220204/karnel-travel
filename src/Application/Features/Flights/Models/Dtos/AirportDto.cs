using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.Flights.Models.Dtos;
public class AirportDto
{
	public string Name { get; set; }
	public string AirportCode { get; set; }
	public string Timezone { get; set; }
	public string CountryCode { get; set; }
	public string ProvinceCode { get; set; }
}

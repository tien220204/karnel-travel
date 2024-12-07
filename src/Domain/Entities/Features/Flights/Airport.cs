using KarnelTravel.Domain.Common;
using System.Xml.Linq;
using System;

namespace KarnelTravel.Domain.Entities.Features.Flights;
public class Airport : BaseAuditableEntity<long>
{
    public string Name { get; set; }
    public string IATACode { get; set; }
	public string ICAOCode { get; set; }
    public string Timezone { get; set; }
    public ICollection<Flight> Flights { get; set; }
}

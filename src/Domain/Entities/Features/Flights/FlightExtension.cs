using KarnelTravel.Domain.Common;

namespace KarnelTravel.Domain.Entities.Features.Flights;
public class FlightExtension : BaseAuditableEntity<long>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public long FlightId { get; set; }
    public Flight Flight { get; set; }
}

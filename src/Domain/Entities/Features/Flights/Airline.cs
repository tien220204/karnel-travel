using KarnelTravel.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Domain.Entities.Features.Flights;
public class Airline : BaseAuditableEntity<long>
{
    public string Name { get; set; }
    public ICollection<Flight> Flights { get; set; }
}

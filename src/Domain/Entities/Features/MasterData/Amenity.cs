using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Enums.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Domain.Entities.Features.MasterData;
public class Amenity : BaseAuditableEntity<long>
{
	public string Name { get; set; }
	public string Description { get; set; }
	public string? Icon { get; set; }
	public AmenityType AmenityType { get; set; }
}

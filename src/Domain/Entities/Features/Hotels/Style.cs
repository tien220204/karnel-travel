using KarnelTravel.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Domain.Entities.Features.Hotels;
public class Style: BaseAuditableEntity<long>
{
	public string Name { get; set; }
	public string Description { get; set; }
	public ICollection<HotelStyle> HotelStyles { get; set; }
}

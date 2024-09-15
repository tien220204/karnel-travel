using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Entities.Features.Hotels;

namespace KarnelTravel.Domain.Entities.Features.MasterData;
public class Ward : BaseEntity<long>
{
	public string Name { get; set; }
	public string Code { get; set; }
	public string ParentCode { get; set; }
    public District District { get; set; }
}

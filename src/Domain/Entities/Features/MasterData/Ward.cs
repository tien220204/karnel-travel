using KarnelTravel.Domain.Common;

namespace KarnelTravel.Domain.Entities.Features.MasterData;
public class Ward : BaseEntity<long>
{
	public string Name { get; set; }
	public string Code { get; set; }
	public string ParentCode { get; set; }
}

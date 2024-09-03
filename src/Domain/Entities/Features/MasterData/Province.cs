using KarnelTravel.Domain.Common;

namespace KarnelTravel.Domain.Entities.Features.MasterData;
public class Province : BaseEntity<long>
{
	public string Name { get; set; }
	public string ShortName { get; set; }
	public string Code { get; set; }
	public int? Priority { get; set; }
}

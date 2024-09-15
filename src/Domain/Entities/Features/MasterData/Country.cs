using KarnelTravel.Domain.Common;

namespace KarnelTravel.Domain.Entities.Features.MasterData;
public class Country : BaseEntity<long>
{
	public string Name { get; set; }
	public string Code { get; set; } 
    public ICollection<Province> Provinces { get; set; }
}

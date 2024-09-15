using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Entities.Features.Hotels;

namespace KarnelTravel.Domain.Entities.Features.MasterData;
public class Province : BaseEntity<long>
{
	public string Name { get; set; }
	public string ShortName { get; set; }
	public string Code { get; set; }
    public string ParentCode { get; set; }  
    public int? Priority { get; set; }
    public Country Country { get; set; }
    public ICollection<District> Districts { get; set; }
}

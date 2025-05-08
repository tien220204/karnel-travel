using KarnelTravel.Domain.Enums.MasterData;
using Share.Common.Models;

namespace KarnelTravel.Application.Features.MasterData.Dtos;
public class AmenityDto : BaseAuditableEntityDto<long>
{
	public string Name { get; set; }
	public string Description { get; set; }
	public string? Icon { get; set; }
	public AmenityType AmenityType { get; set; }
}

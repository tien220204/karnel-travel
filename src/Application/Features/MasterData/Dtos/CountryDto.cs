using Share.Common.Models;

namespace KarnelTravel.Application.Features.MasterData.Dtos;
public class CountryDto : BaseAuditableEntityDto<long>
{
	public string Name { get; set; }
	public string Code { get; set; }
}

using Share.Common.Models;

namespace KarnelTravel.Application.Features.Hotels.Models.Dtos;
public class HotelPolicyDto : BaseAuditableEntityDto<long>
{
	public string Type { get; set; }
	public string Description { get; set; }
}

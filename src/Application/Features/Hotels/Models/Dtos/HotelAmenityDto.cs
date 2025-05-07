using Share.Common.Models;

namespace KarnelTravel.Application.Features.Hotels.Models.Dtos;
public class HotelAmenityDto : BaseAuditableEntityDto<long>
{
	public string Name { get; set; }
	public string Description { get; set; }
}

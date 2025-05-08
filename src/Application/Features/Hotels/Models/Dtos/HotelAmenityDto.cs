using KarnelTravel.Application.Features.MasterData.Dtos;
using Share.Common.Models;

namespace KarnelTravel.Application.Features.Hotels.Models.Dtos;
public class HotelAmenityDto : BaseAuditableEntityDto<long>
{
	public AmenityDto Amenity { get; set; }
}

using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Domain.Entities.Features.MasterData;
using KarnelTravel.Domain.Enums.Hotels;
using Share.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.Hotels.Models.Dtos;
public class HotelDto : BaseAuditableEntityDto<long>
{
	public string Name { get; set; }
	public PropertyType PropertyType { get; set; }
	public HotelClass HotelClass { get; set; }
	public string CountryCode { get; set; }
	public string WardCode { get; set; }
	public string ProvinceCode { get; set; }
	public string DistrictCode { get; set; }
	public long Longitude { get; set; }
	public long Latitude { get; set; }
	public List<PaymentType> PaymentTypes { get; set; }
	public List<ServedMeal> ServedMeals { get; set; }
	public ICollection<HotelPolicy> HotelPolicies { get; set; }
	public Country Country { get; set; }
	public Province Province { get; set; }
	public District District { get; set; }
	public Ward Ward { get; set; }
	public ICollection<HotelImageDto> HotelImages { get; set; }
	public ICollection<HotelAmenityDto> HotelAmenities { get; set; }
	public ICollection<HotelRoomDto> HotelRooms { get; set; }
	public ICollection<HotelReviewDto> HotelReviews { get; set; }
	public ICollection<HotelStyleDto> HotelStyles { get; set; }
}


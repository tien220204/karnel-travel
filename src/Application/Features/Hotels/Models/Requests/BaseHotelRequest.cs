using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Domain.Entities.Features.MasterData;
using KarnelTravel.Domain.Enums.Hotels;

namespace KarnelTravel.Application.Features.Hotel.Models.Requests;
public class BaseHotelRequest
{
	public string Name { get; set; }
	public PropertyType PropertyType { get; set; }
	public HotelClass HotelClass { get; set; }
	public string CountryCode { get; set; }
	public string WardCode { get; set; }
	public string ProvinceCode { get; set; }
	public string DistrictCode { get; set; }
	public IList<PaymentType> PaymentTypes { get; set; }
	public IList<ServedMeal> ServedMeals { get; set; }
	public IList<HotelPolicy> HotelPolicies { get; set; }
	public Country Country { get; set; }
	public Province Province { get; set; }
	public District District { get; set; }
	public Ward Ward { get; set; }
	public IList<HotelImage> HotelImages { get; set; }
	public IList<HotelAmenity> HotelAmenities { get; set; }
	public IList<HotelRoom> HotelRooms { get; set; }
	public IList<HotelReview> HotelReviews { get; set; }
	public IList<HotelStyle> HotelStyles { get; set; }
}

public class CreateHotelRequest : BaseHotelRequest
{

}

public class UpdateHotelRequest : BaseHotelRequest
{
    public string HotelId { get; set; }
}
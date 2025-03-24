using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Domain.Entities.Features.MasterData;
using KarnelTravel.Domain.Enums.Hotels;
using System.ComponentModel.DataAnnotations;

namespace KarnelTravel.Application.Features.Hotel.Models.Requests;
public class BaseHotelRequest
{
	[Required(ErrorMessage = "Tên khách sạn không được để trống.")]
	public string Name { get; set; }
	public PropertyType PropertyType { get; set; }
	public HotelClass HotelClass { get; set; }
	public string CountryCode { get; set; }
	public string WardCode { get; set; }
	public string ProvinceCode { get; set; }
	public string DistrictCode { get; set; }
	[Range(0, 180, ErrorMessage = "Kinh độ phải nằm trong khoảng từ 0 đến 180 độ.")]
	public long Longitude { get; set; }
	[Range(0, 180, ErrorMessage = "Vĩ độ phải nằm trong khoảng từ 0 đến 180 độ.")]
	public long Latitude { get; set; }
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
	public IList<HotelStyle> HotelStyles { get; set; }
}

public class CreateHotelRequest : BaseHotelRequest
{

}

public class  UpdateHotelRequest : BaseHotelRequest
{
    public long HotelId { get; set; }
}
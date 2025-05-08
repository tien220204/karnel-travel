using KarnelTravel.Application.Features.Hotels.Models.Requests;
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
	public PaymentType PaymentType { get; set; }
	public List<ServedMeal> ServedMeals { get; set; }
	public IList<CreateHotelPolicyRequest> HotelPolicies { get; set; }
	public IList<CreateHotelImageRequest> HotelImages { get; set; }
	public IList<CreateHotelAmenityRequest> HotelAmenities { get; set; }
	public IList<CreateHotelStyleRequest> HotelStyles { get; set; }
}

public class CreateHotelRequest : BaseHotelRequest
{

}

public class UpdateHotelRequest : BaseHotelRequest
{
	public long HotelId { get; set; }
}
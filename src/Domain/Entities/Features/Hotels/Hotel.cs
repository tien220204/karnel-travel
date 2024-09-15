using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Entities.Features.MasterData;
using KarnelTravel.Domain.Enums.Hotels;

namespace KarnelTravel.Domain.Entities.Features.Hotels;
public class Hotel : BaseAuditableEntity<Guid>
{
	public string Name { get; set; }
	public PropertyType PropertyType { get; set; }
	public HotelClass HotelClass { get; set; }
    public string CountryCode { get; set; }
	public string WardCode { get; set; }
	public string ProvinceCode { get; set; }
	public string DistrictCode { get; set; }
    public List<PaymentType> PaymentTypes { get; set; }
	public List<ServedMeal> ServedMeals { get; set; }
	public ICollection<HotelPolicy> HotelPolicies { get; set; }
    public Country Country { get; set; }
    public Province Province { get; set; }
    public District District { get; set; }
    public Ward Ward { get; set; }
    public ICollection<HotelImage> HotelImages { get; set; }
    public ICollection<HotelAmenity> HotelAmenities { get; set; }
	public ICollection<HotelRoom> HotelRooms { get; set; }
    public ICollection<HotelReview> HotelReviews { get; set; }
    public ICollection<HotelStyle> HotelStyles { get; set; }
    //public ICollection<HotelPropertyType> HotelProprtyTypes { get; set; }
}

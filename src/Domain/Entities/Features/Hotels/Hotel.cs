using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Entities.Features.MasterData;
using KarnelTravel.Domain.Enums.Hotels;

namespace KarnelTravel.Domain.Entities.Features.Hotels;
public class Hotel : BaseAuditableEntity<Guid>
{
	public string Name { get; set; }
	public PropertyType PropertyType { get; set; }
	public HotelClass HotelClass { get; set; }
    public long CountryId { get; set; }
	public long WardId { get; set; }
	public long ProvinceId { get; set; }
	public long DistrictId { get; set; }
    public ICollection<PaymentType> PaymentTypes { get; set; }
	public ICollection<ServedMeal> ServedMeals { get; set; }
	public ICollection<HotelPolicy> HotelPolicies { get; set; }
    public Country Country { get; set; }
    public Province Provtrict { get; set; }
    public Ward Ward { get; set; }
    public ICollection<HotelImage> HotelImages { get; set; }
    public ICollection<HotelAmenity> ProductAmenities { get; set; }
	public ICollection<HotelRoom> HotelRooms { get; set; }
    public ICollection<HotelReview> HotelReviews { get; set; }
    public ICollection<HotelStyle> HotelStyles { get; set; }
    public ICollection<HotelPropertyType> HotelProprtyTypes { get; set; }
}

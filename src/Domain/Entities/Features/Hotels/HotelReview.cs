using KarnelTravel.Domain.Common;

namespace KarnelTravel.Domain.Entities.Features.Hotels;
public class HotelReview : BaseAuditableEntity<long>
{
    public decimal StarRate { get; set; }
    public string Review { get; set; }
    public long HotelId { get; set; }
    public long ParentReviewId { get; set; }
    //public Guid CustomerId { get; set; }
    //public Customer Customer { get; set; }
    public HotelReview ParentReview { get; set; }
    public ICollection<HotelReview> ChildReviews { get; set; }
    public Hotel Hotel { get; set; }
}

using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Entities.Features.Users;

namespace KarnelTravel.Domain.Entities.Features.Hotels
{
    public class HotelReview : BaseAuditableEntity<long>
    {
        public string Review { get; set; }
        public long HotelId { get; set; }
        public long ParentReviewId { get; set; }
        public Guid UserId { get; set; }
        public HotelReview ParentReview { get; set; }
        public ICollection<HotelReview> ChildReviews { get; set; }
        public ApplicationUser User { get; set; }
        public Hotel Hotel { get; set; }
    }
}
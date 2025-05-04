using Share.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.Hotels.Models.Dtos;
public class HotelReviewDto : BaseAuditableEntityDto<long>
{
	public string Review { get; set; }
	public string UserId { get; set; }
	public string UserName { get; set; }
	public string UserAvatar { get; set; }
	public long ParentReviewId { get; set; }
	public List<HotelReviewDto> ChildReviews { get; set; }
}

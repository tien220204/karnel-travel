using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Common.Security;
using KarnelTravel.Share.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace KarnelTravel.Application.Features.Hotels.Commands.HotelReview;

[Authorize(Roles = "user")]
public class UpdateHotelReviewCommand : IRequest<AppActionResultData<string>>
{
	public long HotelReviewId { get; set; }
	public string Review { get; set; }
}

public class UpdateHotelReviewCommandHandler : BaseHandler, IRequestHandler<UpdateHotelReviewCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IFusionCache _fusionCache;
	public UpdateHotelReviewCommandHandler(IApplicationDbContext context, IFusionCache fusionCache)
	{
		_context = context;
		_fusionCache = fusionCache;
	}
	public async Task<AppActionResultData<string>> Handle(UpdateHotelReviewCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();

		var hotelReview = _context.HotelReviews.AsNoTracking().FirstOrDefault(x => x.Id == request.HotelReviewId && !x.IsDeleted);

		if (hotelReview is null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, request.HotelReviewId);
		}

		hotelReview.Review = request.Review;

		_context.HotelReviews.Update(hotelReview);
		await _context.SaveChangesAsync(cancellationToken);


		//await _fusionCache.RemoveAsync(CacheKeys.ALL_PRODUCT_CATEGORY);

		return BuildMultilingualResult(result, hotelReview.Id.ToString(), Resources.INF_MSG_SUCCESSFULLY);
	}
}

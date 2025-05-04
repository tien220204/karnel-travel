using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.Hotel.Models.Requests;
using KarnelTravel.Share.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace KarnelTravel.Application.Features.Hotels.Commands.HotelRating;


public class UpdateHotelRatingCommand : IRequest<AppActionResultData<string>>
{
	public long Id { get; set; }
	public decimal StarRate { get; set; }
}

public class UpdateHotelRatingCommandHandler : BaseHandler, IRequestHandler<UpdateHotelRatingCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IFusionCache _fusionCache;
	public UpdateHotelRatingCommandHandler(IApplicationDbContext context, IFusionCache fusionCache)
	{
		_context = context;
		_fusionCache = fusionCache;
	}
	public async Task<AppActionResultData<string>> Handle(UpdateHotelRatingCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();

		var hotelRating = _context.HotelRatings.AsNoTracking().FirstOrDefault(x => x.Id == request.Id && !x.IsDeleted);

		if (hotelRating is null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, request.Id);
		}

		hotelRating.StarRate = request.StarRate;


		_context.HotelRatings.Update(hotelRating);
		await _context.SaveChangesAsync(cancellationToken);

		//await _fusionCache.RemoveAsync(CacheKeys.ALL_PRODUCT_CATEGORY);

		return BuildMultilingualResult(result, hotelRating.Id.ToString(), Resources.INF_MSG_SUCCESSFULLY);
	}
}
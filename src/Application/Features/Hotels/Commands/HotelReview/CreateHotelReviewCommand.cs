﻿using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Features.Hotel.Models.Requests;
using KarnelTravel.Share.Localization;
using MediatR;
using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Application.Common.Security;
using System.ComponentModel.DataAnnotations;
using ZiggyCreatures.Caching.Fusion;
using KarnelTravel.Share.Cache.Contanst;

namespace KarnelTravel.Application.Features.Hotels.Commands.HotelReview;

[Authorize(Roles = "user")]
public class CreateHotelReviewCommand :  IRequest<AppActionResultData<string>>
{
	public string Review { get; set; }

	[Required]
	public int HotelId { get; set; }

	[Required]
	public string UserId { get; set; }
	public long? ParentReviewId { get; set; }
}

public class CreateHotelReviewCommandHandler : BaseHandler, IRequestHandler<CreateHotelReviewCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IFusionCache _fusionCache;

	public CreateHotelReviewCommandHandler(IApplicationDbContext context, IFusionCache fusionCache)
	{
		_context = context;
		_fusionCache = fusionCache;
	}

	public async Task<AppActionResultData<string>> Handle(CreateHotelReviewCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();

		if (!Guid.TryParse(request.UserId, out Guid _guidUserId))
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_INVALID_GUID_ID, request.UserId);
		}

		var newHotelReview = new Domain.Entities.Features.Hotels.HotelReview
		{
			Review = request.Review,
			HotelId = request.HotelId,
			UserId = _guidUserId,
			ParentReviewId = request.ParentReviewId ?? 0,
		};

		_context.HotelReviews.Add(newHotelReview);

		await _fusionCache.RemoveAsync(CacheKeys.ALL_HOLTEL_REVIEW);
		await _context.SaveChangesAsync(cancellationToken);


		return BuildMultilingualResult(result, newHotelReview.Id.ToString(), Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
}
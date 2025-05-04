using AutoMapper;
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.Hotels.Models.Dtos;
using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Share.Cache.Contanst;
using KarnelTravel.Share.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace KarnelTravel.Application.Features.Hotels.Queries;


public record GetAllHotelReviewQuery : IRequest<AppActionResultData<IList<HotelReviewDto>>>
{
}

public class GetAllHotelReviewQueryHandler : BaseHandler, IRequestHandler<GetAllHotelReviewQuery, AppActionResultData<IList<HotelReviewDto>>>
{
	private readonly IApplicationDbContext _context;
	private readonly IFusionCache _fusionCache;
	private readonly IMapper _mapper;

	public GetAllHotelReviewQueryHandler(
		IApplicationDbContext context,
		IFusionCache fusionCache,
		IMapper mapper)
	{
		_context = context;
		_fusionCache = fusionCache;
		_mapper = mapper;
	}

	public async Task<AppActionResultData<IList<HotelReviewDto>>> Handle(GetAllHotelReviewQuery request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<IList<HotelReviewDto>>();

		//var hotelReviewDtos = await _fusionCache.GetOrDefaultAsync<IList<HotelReviewDto>>(CacheKeys.ALL_PRODUCT_CATEGORY);
		

		//if (hotelReviewDtos.IsNullOrEmpty())
		//{
			var hotelReviews = await _context.HotelReviews.Include(x => x.ParentReview)
												.Include(x => x.ChildReviews).AsNoTracking()
												.Where(x => !x.IsDeleted)
												.ToListAsync();

			var hotelReviewDtos = BuildHierarchy(hotelReviews, null);

			//await _fusionCache.SetAsync(CacheKeys.ALL_PRODUCT_CATEGORY_ACTIVE, HotelReviewDtos);
		//}
		return BuildMultilingualResult(result, hotelReviewDtos, Resources.INF_MSG_SUCCESSFULLY);
	}

	private List<HotelReviewDto> BuildHierarchy(List<HotelReview> reviews, long? parentId)
	{
		var result = reviews
			.Where(c => c.ParentReviewId == parentId)
			.Select(c => new HotelReviewDto
			{
				Id = c.Id,
				Review = c.Review,
				ParentReviewId = c.ParentReviewId,
				UserId = c.UserId.ToString(),
				ChildReviews = BuildHierarchy(reviews, c.Id),
				Created = c.Created,
				CreatedBy = c.CreatedBy,
				LastModified = c.LastModified,
				LastModifiedBy = c.LastModifiedBy,
				
			})
			.OrderByDescending(c => c.Created)
			.ToList();
		//categories.RemoveAll(c => result.Any(r => r.Id == c.Id));
		return result;
	}
}

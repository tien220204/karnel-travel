using AutoMapper;
using AutoMapper.QueryableExtensions;
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Common.Mappings;
using KarnelTravel.Application.Features.Hotels.Models.Dtos;
using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Share.Localization;
using MediatR;

namespace KarnelTravel.Application.Features.Hotels.Queries;
public class GetHotelRoomWithFilterAndPaginationQuery : IRequest<AppActionResultData<Common.Models.PaginatedList<HotelRoomDto>>>
{
	public string SearchText { get; set; }
	public int PageIndex { get; set; } = 1;
	public int PageSize { get; set; } = 10;
}

public class GetHotelRoomWithFilterAndPaginationQueryHandler : BaseHandler, IRequestHandler<GetHotelRoomWithFilterAndPaginationQuery, AppActionResultData<Common.Models.PaginatedList<HotelRoomDto>>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly IElasticSearchService _elasticSearchService;

	public GetHotelRoomWithFilterAndPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IElasticSearchService elasticService)
	{
		_context = context;
		_mapper = mapper;
		_elasticSearchService = elasticService;
	}
	public async Task<AppActionResultData<Common.Models.PaginatedList<HotelRoomDto>>> Handle(GetHotelRoomWithFilterAndPaginationQuery request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<Common.Models.PaginatedList<HotelRoomDto>>();
		var query = _context.HotelRooms.AsQueryable();

		var searchText = request.SearchText?.Trim().ToLower() ?? string.Empty;


		//search bt keyword on elastic search
		var fieldToSearch = new List<string>
		{
			nameof(HotelRoom.Code),
			nameof(HotelRoom.Description),
			nameof(HotelRoom.PricePerHour),
			nameof(HotelRoom.Capacity),

		};

		var elasticResult = await _elasticSearchService.SearchMultiFieldsByKeyword<HotelRoomDto>(fieldToSearch, searchText, nameof(HotelRoom));

		var res = elasticResult.Hits.Select(x => x.Source).ToList();

		var response = await res.OrderByDescending(x => x.Created)
			.PaginatedListAsync(request.PageIndex, request.PageSize);

		//var response = await query
		//	.OrderByDescending(x => x.Created)
		//	.ProjectTo<HotelRoomDto>(_mapper.ConfigurationProvider)
		//	.PaginatedListAsync(request.PageIndex, request.PageSize);

		return BuildMultilingualResult(result, response, Resources.INF_MSG_SUCCESSFULLY);
	}
}
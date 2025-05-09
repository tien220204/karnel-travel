using AutoMapper;
using AutoMapper.QueryableExtensions;
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Common.Mappings;
using KarnelTravel.Application.Features.Hotels.Models.Dtos;
using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Share.Localization;
using MediatR;
using Share.Common.Extensions;
using System.Net.WebSockets;
using HotelClass = KarnelTravel.Domain.Entities.Features.Hotels.Hotel;


namespace KarnelTravel.Application.Features.Hotels.Queries;

//[Authorize(Roles = "user")]
public class GetHotelWithFilterAndPaginationQuery : IRequest<AppActionResultData<Common.Models.PaginatedList<HotelDto>>>
{
	public string SearchText { get; set; }
	public int PageIndex { get; set; } = 1;
	public int PageSize { get; set; } = 10;
}


public class GetHotelWithFilterAndPaginationQueryHandler : BaseHandler, IRequestHandler<GetHotelWithFilterAndPaginationQuery, AppActionResultData<Common.Models.PaginatedList<HotelDto>>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly IElasticSearchService _elasticSearchService;
	public GetHotelWithFilterAndPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IElasticSearchService elasticSearchService)
	{
		_context = context;
		_mapper = mapper;
		_elasticSearchService = elasticSearchService;
	}

	public async Task<AppActionResultData<Common.Models.PaginatedList<HotelDto>>> Handle(GetHotelWithFilterAndPaginationQuery request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<Common.Models.PaginatedList<HotelDto>>();

		//query for main db
		//var query = _context.Hotels.AsQueryable();

		var searchText = request.SearchText?.Trim().ToLower() ?? string.Empty;

		//search keyword with hotel in postgre
		//if (request.SearchText.IsNotNullNorEmpty())
		//{
		//	query = query.Where(x => x.Name.ToLower().Contains(searchText));
		//}


		//search bt keyword on elastic search
		var fieldToSearch = new List<string>
		{
			"id",
			//"_id",
			//"description",
			//"district.name"
		};

		var elasticResult = await _elasticSearchService.SearchMultiFieldsByKeyword<HotelDto>(fieldToSearch, searchText, nameof(Hotel), request.PageIndex, request.PageSize);

		var res = elasticResult.Hits.Select(x => x.Source).ToList();

		var response = await res.OrderByDescending(x => x.Created)
			.PaginatedListAsync(request.PageIndex, request.PageSize);


		//var response = await query
		//	.OrderByDescending(x => x.Created)
		//	.ProjectTo<HotelDto>(_mapper.ConfigurationProvider)
		//	.PaginatedListAsync(request.PageIndex, request.PageSize);


		return BuildMultilingualResult(result, response, Resources.INF_MSG_SUCCESSFULLY);
	}
}
using AutoMapper;
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Common.Mappings;
using KarnelTravel.Application.Features.Flights.Models.Dtos;
using KarnelTravel.Domain.Entities.Features.Flights;
using KarnelTravel.Share.Localization;
using MediatR;

namespace KarnelTravel.Application.Features.Flights.Queries;

public class GetFlightsWithFilterAndPaginationQuery : IRequest<AppActionResultData<Common.Models.PaginatedList<FlightDto>>>
{
	public string SearchText { get; set; }
	public int PageIndex { get; set; } = 1;
	public int PageSize { get; set; } = 10;
}


public class GetFlightsWithFilterAndPaginationQueryHandler : BaseHandler, IRequestHandler<GetFlightsWithFilterAndPaginationQuery, AppActionResultData<Common.Models.PaginatedList<FlightDto>>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly IElasticSearchService _elasticSearchService;
	public GetFlightsWithFilterAndPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IElasticSearchService elasticSearchService)
	{
		_context = context;
		_mapper = mapper;
		_elasticSearchService = elasticSearchService;
	}

	public async Task<AppActionResultData<Common.Models.PaginatedList<FlightDto>>> Handle(GetFlightsWithFilterAndPaginationQuery request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<Common.Models.PaginatedList<FlightDto>>();

		var query = _context.Flights.AsQueryable();

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

		var elasticResult = await _elasticSearchService.SearchMultiFieldsByKeyword<FlightDto>(fieldToSearch, searchText, nameof(Flight));

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
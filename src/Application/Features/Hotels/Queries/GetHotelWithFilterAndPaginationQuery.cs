using AutoMapper;
using AutoMapper.QueryableExtensions;
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.Hotels.Models.Dtos;
using KarnelTravel.Share.Localization;
using MediatR;
using Share.Common.Extensions;
using KarnelTravel.Application.Common.Mappings;
using KarnelTravel.Application.Common.Security;


namespace KarnelTravel.Application.Features.Hotels.Queries;

[Authorize(Roles = "admin")]
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
	public GetHotelWithFilterAndPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<AppActionResultData<Common.Models.PaginatedList<HotelDto>>> Handle(GetHotelWithFilterAndPaginationQuery request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<Common.Models.PaginatedList<HotelDto>>();

		var query = _context.Hotels.AsQueryable();
		
		var searchText = request.SearchText?.Trim().ToLower() ?? string.Empty;

		//search keyword with hotel in postgre
		if(request.SearchText.IsNotNullNorEmpty())
		{
			query = query.Where(x => x.Name.ToLower().Contains(searchText)) ;
		}

		var response = await query
			.OrderByDescending(x => x.Created)
			.ProjectTo<HotelDto>(_mapper.ConfigurationProvider)
			.PaginatedListAsync(request.PageIndex, request.PageSize);


		return BuildMultilingualResult(result, response, Resources.INF_MSG_SUCCESSFULLY);
	}
}
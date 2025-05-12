using Amazon.Auth.AccessControlPolicy;
using AutoMapper;
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.Flights.Models.Requests;
using KarnelTravel.Share.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace KarnelTravel.Application.Features.Flights.Commands.Airports;
public class UpdateAirportCommand : UpdateAirportRequest, IRequest<AppActionResultData<string>>
{
}

public class UpdateAirportCommandHandler : BaseHandler, IRequestHandler<UpdateAirportCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IElasticSearchService _elasticSearchService;
	private readonly IMapper _mapper;
	private readonly IFusionCache _cache;
	public UpdateAirportCommandHandler(IApplicationDbContext context, IElasticSearchService elasticSearchService, IMapper mapper, IFusionCache cache)
	{
		_context = context;
		_elasticSearchService = elasticSearchService;
		_mapper = mapper;
		_cache = cache;
	}
	
	public async Task<AppActionResultData<string>> Handle(UpdateAirportCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();

		var country = await _context.Countries.Include(c => c.Provinces).FirstOrDefaultAsync(c => c.Code == request.CountryCode);

		if (country is null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, nameof(request.CountryCode));
		}

		var province = country.Provinces.FirstOrDefault(c => c.Code == request.ProvinceCode);

		if (province is null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, nameof(request.ProvinceCode));
		}

		var airport = await _context.Airports.FindAsync(request.Id);

		if (airport is null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, request.Id);
		}
		airport.Name = request.Name;
		airport.AirportCode = request.AirportCode;
		airport.ProvinceCode = request.ProvinceCode;
		airport.CountryCode = request.CountryCode;
		airport.Timezone = request.Timezone;

		await _context.SaveChangesAsync(cancellationToken);
		return BuildMultilingualResult(result, airport.Id.ToString(), Resources.INF_MSG_SUCCESSFULLY);
	}
}
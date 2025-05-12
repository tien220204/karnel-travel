using Amazon.Auth.AccessControlPolicy;
using AutoMapper;
using Elastic.Clients.Elasticsearch.MachineLearning;
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.Flights.Models.Requests;
using KarnelTravel.Domain.Entities.Features.Flights;
using KarnelTravel.Domain.Entities.Features.MasterData;
using KarnelTravel.Share.Cache.Contanst;
using KarnelTravel.Share.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace KarnelTravel.Application.Features.Flights.Commands.Airports;
public class CreateAirportCommand : CreateAirportRequest, IRequest<AppActionResultData<string>>
{
}

public class CreateAirportCommandHandler : BaseHandler, IRequestHandler<CreateAirportCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IElasticSearchService _elasticSearchService;
	private readonly IMapper _mapper;
	private readonly IFusionCache _cache;
	public CreateAirportCommandHandler(IApplicationDbContext context, IElasticSearchService elasticSearchService, IMapper mapper, IFusionCache cache)
	{
		_context = context;
		_elasticSearchService = elasticSearchService;
		_mapper = mapper;
		_cache = cache;
	}

	public async Task<AppActionResultData<string>> Handle(CreateAirportCommand request, CancellationToken cancellationToken)
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

		var newAirport = new Airport
		{
			AirportCode = request.AirportCode,
			Name = request.Name,
			ProvinceCode = request.ProvinceCode,
			CountryCode = request.CountryCode,
			Timezone = request.Timezone,
		}
		

		await _context.Airports.AddAsync(newAirport);

		//await _cache.RemoveAsync(CacheKeys.);
		await _context.SaveChangesAsync(cancellationToken);



		return BuildMultilingualResult(result, newAirport.Id.ToString(), Resources.INF_MSG_SUCCESSFULLY);
	}
}
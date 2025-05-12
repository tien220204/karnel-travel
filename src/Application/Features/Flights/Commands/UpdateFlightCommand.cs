using AutoMapper;
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.Flights.Models.Requests;
using KarnelTravel.Application.Features.Hotels.Models.Dtos;
using KarnelTravel.Domain.Entities.Features.Flights;
using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Domain.Enums.Hotels;
using KarnelTravel.Share.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Common.Extensions;
using ZiggyCreatures.Caching.Fusion;

namespace KarnelTravel.Application.Features.Flights.Commands;


public class UpdateFlightCommand : UpdateFlightRequest, IRequest<AppActionResultData<string>>
{
}

public class UpdateFlightCommandHandler : BaseHandler, IRequestHandler<UpdateFlightCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IFusionCache _fusionCache;
	private readonly IElasticSearchService _elasticSearchService;
	private readonly IMapper _mapper;
	public UpdateFlightCommandHandler(IApplicationDbContext context, IElasticSearchService elasticSearchService, IMapper mapper, IFusionCache fusionCache)
	{
		_context = context;
		_elasticSearchService = elasticSearchService;
		_mapper = mapper;
		_fusionCache = fusionCache;
	}

	public async Task<AppActionResultData<string>> Handle(UpdateFlightCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();

		var flight = _context.Flights.Include(f => f.FlightTickets).FirstOrDefault(f => f.Id == request.Id && !f.IsDeleted);

		if (flight == null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, request.Id.ToString());
		}

		if(CanUpdateFlight(flight))
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_UNABLE_TO_MODIFY_DATA, [nameof(Flight), request.Id] );
		}

		flight.Name = request.Name;
		flight.FlightCode = request.FlightCode;
		flight.DepartureAirportId = request.DepartureAirportId;
		flight.ArrivalAirportId = request.ArrivalAirportId;

		//await _fusionCache.RemoveAsync(CacheKeys.ALL_PRODUCT_CATEGORY);

		return BuildMultilingualResult(result, flight.Id.ToString(), Resources.INF_MSG_SUCCESSFULLY);
	}

	private bool CanUpdateFlight(Flight flight)
	{
		return flight.FlightTickets.IsNullOrEmpty();
	}
}
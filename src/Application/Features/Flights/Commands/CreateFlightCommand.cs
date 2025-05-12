using AutoMapper;
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.Flights.Models.Dtos;
using KarnelTravel.Application.Features.Flights.Models.Requests;
using KarnelTravel.Domain.Entities.Features.Flights;
using KarnelTravel.Share.Localization;
using MediatR;

namespace KarnelTravel.Application.Features.Flights.Commands;
public class CreateFlightCommand : CreateFlightRequest, IRequest<AppActionResultData<string>>
{
}

public class CreateFlightCommandHand : BaseHandler, IRequestHandler<CreateFlightCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IElasticSearchService _elasticSearchService;
	private readonly IMapper _mapper;

	public CreateFlightCommandHand(IApplicationDbContext context, IElasticSearchService elasticSearchService, IMapper mapper)
	{
		_context = context;
		_elasticSearchService = elasticSearchService;
		_mapper = mapper;
	}
	public async Task<AppActionResultData<string>> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();

		var flight = new Domain.Entities.Features.Flights.Flight
		{
			FlightCode = request.FlightCode,
			Name = request.Name,
			AirlineId = request.AirlineId,
			DepartureAirportId = request.DepartureAirportId,
			ArrivalAirportId = request.ArrivalAirportId,
		};

		_context.Flights.Add(flight);

		await _context.SaveChangesAsync(cancellationToken);

		var flightDto = _mapper.Map<FlightDto>(flight);
		await _elasticSearchService.AddOrUpdate(flightDto, nameof(Flight));

		return BuildMultilingualResult(result, flight.Id.ToString(), Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
}

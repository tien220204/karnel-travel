using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Domain.Entities.Features.Flights;
using KarnelTravel.Share.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KarnelTravel.Application.Features.Flights.Commands;
public class DeleteFlightCommand : IRequest<AppActionResultData<string>>
{
	public int Id { get; set; }
}

public class DeleteFlightCommandHandler : BaseHandler, IRequestHandler<DeleteFlightCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	public DeleteFlightCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}
	public async Task<AppActionResultData<string>> Handle(DeleteFlightCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();
		var flight = await _context.Flights.Include(f => f.FlightTickets).FirstOrDefaultAsync(f => f.Id == request.Id && !f.IsDeleted);

		if (flight == null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, request.Id.ToString());
		}

		if (CanUpdateFlight(flight){
			return BuildMultilingualError(result, Resources.ERR_MSG_UNABLE_TO_MODIFY_DATA, [nameof(Flight), request.Id]);
		}

		flight.IsDeleted = true;

		_context.Flights.Update(flight);
		await _context.SaveChangesAsync(cancellationToken);

		return BuildMultilingualResult(result, request.Id.ToString(), Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
	private bool CanUpdateFlight(Flight flight)
	{
		return flight.FlightTickets.IsNullOrEmpty();
	}
}

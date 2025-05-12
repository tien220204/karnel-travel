using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Share.Localization;
using MediatR;

namespace KarnelTravel.Application.Features.Flights.Commands.Airports;
public class DeleteAirportCommand : IRequest<AppActionResultData<string>>
{
	public int Id { get; set; }
}

public class DeleteAirportCommandHandler : BaseHandler, IRequestHandler<DeleteAirportCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	public DeleteAirportCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}
	public async Task<AppActionResultData<string>> Handle(DeleteAirportCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();
		var airport = await _context.Airports.FindAsync(request.Id);
		if (airport == null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, request.Id.ToString());
		}

		airport.IsDeleted = true;
		_context.Airports.Update(airport);
		await _context.SaveChangesAsync(cancellationToken);

		//update cache

		return BuildMultilingualResult(result, request.Id.ToString(), Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
}

using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Common.Security;
using KarnelTravel.Share.Localization;
using MediatR;

namespace KarnelTravel.Application.Features.Hotels.Commands.HotelRating;


[Authorize(Roles = "user,admin")]
public record DeleteHotelRatingCommand : IRequest<AppActionResultData<string>>
{
	public long Id { get; set; }
}

public class DeleteHotelRatingCommandHandler : BaseHandler, IRequestHandler<DeleteHotelRatingCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;

	public DeleteHotelRatingCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<AppActionResultData<string>> Handle(DeleteHotelRatingCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();

		var hotelRating = _context.HotelRatings.FirstOrDefault(x => x.Id == request.Id && !x.IsDeleted);

		if (hotelRating is null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, request.Id);
		}

		hotelRating.IsDeleted = true;

		_context.HotelRatings.Update(hotelRating);

		await _context.SaveChangesAsync(cancellationToken);

		return BuildMultilingualResult(result, request.Id.ToString(), Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
}

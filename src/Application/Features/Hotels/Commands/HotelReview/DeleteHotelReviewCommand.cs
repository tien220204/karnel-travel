using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Common.Security;
using KarnelTravel.Share.Localization;
using MediatR;
namespace KarnelTravel.Application.Features.Hotels.Commands.HotelReview;

[Authorize(Roles = "user")]
public record DeleteHotelReviewCommand : IRequest<AppActionResultData<string>>
{
	public long Id { get; set; }
}

public class DeleteHotelReviewCommandHandler : BaseHandler, IRequestHandler<DeleteHotelReviewCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;

	public DeleteHotelReviewCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<AppActionResultData<string>> Handle(DeleteHotelReviewCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();

		var hotelReview = _context.HotelReviews.FirstOrDefault(x => x.Id == request.Id && !x.IsDeleted);

		if (hotelReview is null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, request.Id);
		}

		hotelReview.IsDeleted = true;

		_context.HotelReviews.Update(hotelReview);

		await _context.SaveChangesAsync(cancellationToken);

		return BuildMultilingualResult(result, request.Id.ToString(), Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
}
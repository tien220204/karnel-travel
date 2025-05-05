using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Common.Security;
using KarnelTravel.Share.Localization;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace KarnelTravel.Application.Features.Hotels.Commands.HotelRating;


[Authorize(Roles = "user")]
public class CreateHotelRatingCommand : IRequest<AppActionResultData<string>>
{
	public decimal StarRate { get; set; }

	[Required]
	public int HotelId { get; set; }
	
	[Required]	
	public string UserId { get; set; }

}

public class CreateHotelRatingCommandHandler : BaseHandler, IRequestHandler<CreateHotelRatingCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;

	public CreateHotelRatingCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<AppActionResultData<string>> Handle(CreateHotelRatingCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();

		if (!Guid.TryParse(request.UserId, out Guid _guidUserId))
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_INVALID_GUID_ID, request.UserId);
		}

		var newHotelRating = new Domain.Entities.Features.Hotels.HotelRating
		{
			StarRate = request.StarRate,
			HotelId = request.HotelId,
			UserId = _guidUserId,

		};


		_context.HotelRatings.Add(newHotelRating);
		await _context.SaveChangesAsync(cancellationToken);

		return BuildMultilingualResult(result, newHotelRating.Id.ToString(), Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
}

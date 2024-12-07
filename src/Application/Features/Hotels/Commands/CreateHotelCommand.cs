using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.Hotel.Models.Requests;
using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Share.Localization;
using MediatR;

namespace KarnelTravel.Application.Feature.Hotels;
public class CreateHotelCommand : CreateHotelRequest, IRequest<AppActionResultData<string>>
{
}

public class CreateHotelCommandHandler : BaseHandler, IRequestHandler<CreateHotelCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;

	public CreateHotelCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<AppActionResultData<string>> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();


		var newHotel = new Hotel();
		{

		};



		_context.Hotels.Add(newHotel);
		await _context.SaveChangesAsync(cancellationToken);

		return BuildMultilingualResult(result, newHotel.Id.ToString(), Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
}
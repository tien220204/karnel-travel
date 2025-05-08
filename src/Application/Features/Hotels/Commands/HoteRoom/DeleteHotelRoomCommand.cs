using AutoMapper;
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Share.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KarnelTravel.Application.Features.Hotels.Commands.HoteRoom;
public class DeleteHotelRoomCommand : IRequest<AppActionResultData<string>>
{
	public long Id { get; set; }
}

public class DeleteHotelRoomCommandHandler : BaseHandler, IRequestHandler<DeleteHotelRoomCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IElasticSearchService _elasticSearchService;
	private readonly IMapper _mapper;

	public DeleteHotelRoomCommandHandler(IApplicationDbContext context, IElasticSearchService elasticSearchService, IMapper mapper)
	{
		_context = context;
		_elasticSearchService = elasticSearchService;
		_mapper = mapper;
	}
	public async Task<AppActionResultData<string>> Handle(DeleteHotelRoomCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();

		var hotelRoom = await _context.HotelRooms.FirstOrDefaultAsync(r => !r.IsDeleted && r.Id == request.Id);

		if (hotelRoom is null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, request.Id);
		}

		hotelRoom.IsDeleted = true;

		await _context.SaveChangesAsync(cancellationToken);

		//delete room data in elastic search
		_mapper.Map<HotelRoom>(hotelRoom);
		await _elasticSearchService.Remove<HotelRoom>(hotelRoom.Id.ToString(), nameof(HotelRoom).ToLower());


		return BuildMultilingualResult(result, hotelRoom.Id.ToString(), Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
}

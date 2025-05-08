using AutoMapper;
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.Hotels.Models.Requests;
using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Share.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KarnelTravel.Application.Features.Hotels.Commands.HoteRoom;
public class UpdateHotelRoomCommand : UpdateHotelRoomRequest, IRequest<AppActionResultData<string>>
{
}

public class UpdateHotelRoomCommandHandler : BaseHandler, IRequestHandler<UpdateHotelRoomCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly IElasticSearchService _elasticSearchService;

	public UpdateHotelRoomCommandHandler(IApplicationDbContext context, IMapper mapper, IElasticSearchService elasticSearchService)
	{
		_context = context;
		_mapper = mapper;
		_elasticSearchService = elasticSearchService;
	}

	public async Task<AppActionResultData<string>> Handle(UpdateHotelRoomCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();
		
		var room = await _context.HotelRooms.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted);

		if (room is null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_EXISTED, nameof(request.Id));
		}

		room.PricePerHour = request.PricePerHour;
		room.Description = request.Description;
		room.Capacity = request.Capacity;
		room.Code = request.Code;

		await _context.SaveChangesAsync(cancellationToken);

		//update room in elasticsearch
		var newRoomDto =  _mapper.Map<HotelRoom>(room);
		await _elasticSearchService.CreateIndexIfNotExisted(nameof(HotelRoom));
		await _elasticSearchService.AddOrUpdate(newRoomDto, nameof(HotelRoom));

		return BuildMultilingualResult(result, room.Id.ToString(), Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
}

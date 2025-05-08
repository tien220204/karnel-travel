using AutoMapper;
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.Hotels.Models.Dtos;
using KarnelTravel.Application.Features.Hotels.Models.Requests;
using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Share.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KarnelTravel.Application.Features.Hotels.Commands.HoteRoom;
public class CreateHotelRoomCommand : IRequest<AppActionResultData<string>>
{
	public List<CreateHotelRoomRequest> HotelRooms { get; set; }
	public long HotelId { get; set; }
}

public class CreateHotelRoomCommandHandler : BaseHandler, IRequestHandler<CreateHotelRoomCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly IElasticSearchService _elasticSearchService;

	public CreateHotelRoomCommandHandler(IApplicationDbContext context, IMapper mapper, IElasticSearchService elasticSearchService)
	{
		_context = context;
		_mapper = mapper;
		_elasticSearchService = elasticSearchService;
	}
	public async Task<AppActionResultData<string>> Handle(CreateHotelRoomCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();

		var hotel = await _context.Hotels.Include(h => h.HotelRooms).FirstOrDefaultAsync(x => x.Id == request.HotelId && !x.IsDeleted);

		if (hotel is null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, request.HotelId);
		}

		//get all code in request
		var requestRoomCodes = request.HotelRooms.Select(x => x.Code).ToList();

		//check if room existed in db
		var existingRoomCodes = hotel.HotelRooms.Any(r => requestRoomCodes.Contains(r.Code));

		if (existingRoomCodes)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_EXISTED, nameof(request.HotelRooms));
		}

		//add rooms to hotel
		var hotelRooms = request.HotelRooms.Select(x => new HotelRoom
		{
			HotelId = request.HotelId,
			Code = x.Code,
			Description = x.Description,
			PricePerHour = x.PricePerHour,
			Capacity = x.Capacity,

		}).ToList();

		await _context.HotelRooms.AddRangeAsync(hotelRooms);

		await _context.SaveChangesAsync(cancellationToken);

		//update existing hotel rooms list in elastic search
		var roomDtoList  = _mapper.Map<List<HotelRoomDto>>(hotel.HotelRooms);
		await _elasticSearchService.CreateIndexIfNotExisted(nameof(HotelRoom));
		await _elasticSearchService.AddOrUpdateBulk(roomDtoList, nameof(HotelRoom));


		return BuildMultilingualResult(result, hotel.Id.ToString(), Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
}

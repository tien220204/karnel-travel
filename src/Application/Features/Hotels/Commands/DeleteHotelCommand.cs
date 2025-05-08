using AutoMapper;
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.Hotels.Models.Dtos;
using KarnelTravel.Share.Localization;
using MediatR;

namespace KarnelTravel.Application.Features.Hotels.Commands;
public record DeleteHotelCommand : IRequest<AppActionResultData<string>>
{
	public long Id { get; set; }
}

public class DeleteHotelCommandHandler : BaseHandler, IRequestHandler<DeleteHotelCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IElasticSearchService _elasticSearchService;
	private readonly IMapper _mapper

	public DeleteHotelCommandHandler(IApplicationDbContext context, IElasticSearchService elasticSearchService, IMapper mapper)
	{
		_context = context;
		_elasticSearchService = elasticSearchService;
		_mapper = mapper;
	}

	public async Task<AppActionResultData<string>> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();


		var hotel = _context.Hotels.FirstOrDefault(x => x.Id == request.Id && !x.IsDeleted);

		if (hotel is null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, request.Id);
		}

		hotel.IsDeleted = true;

		_context.Hotels.Update(hotel);

		await _context.SaveChangesAsync(cancellationToken);

		//remove index in elastic search
		_mapper.Map<HotelDto>(hotel);
		await _elasticSearchService.Remove<HotelDto>(hotel.Id.ToString(), hotel.GetType().Name);

		return BuildMultilingualResult(result, request.Id.ToString(), Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
}


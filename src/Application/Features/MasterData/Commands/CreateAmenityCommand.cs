using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.Hotels.Commands.HotelRating;
using KarnelTravel.Domain.Entities.Features.MasterData;
using KarnelTravel.Domain.Enums.Hotels;
using KarnelTravel.Domain.Enums.MasterData;
using KarnelTravel.Share.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KarnelTravel.Application.Features.MasterData.Commands;
public class CreateAmenityCommand : IRequest<AppActionResultData<string>>
{
	public string Name { get; set; }
	public string Description { get; set; }
	public AmenityType AmenityType { get; set; }
}
public class CreateAmenityCommandHandler : BaseHandler, IRequestHandler<CreateAmenityCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IElasticSearchService _elasticSearchService;

	public CreateAmenityCommandHandler(IApplicationDbContext context, IElasticSearchService elasticSearchService)
	{
		_context = context;
		_elasticSearchService = elasticSearchService;
	}

	public async Task<AppActionResultData<string>> Handle(CreateAmenityCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();

		//check for is defined enum type : AmenityType
		if (!Enum.IsDefined(typeof(AmenityType), request.AmenityType) || !Enum.TryParse(typeof(AmenityType), request.AmenityType.ToString(), out var amenityType))
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, nameof(request.AmenityType));
		}

		var existingAmenity = await _context.Amenities.AnyAsync(x => x.Name == request.Name && !x.IsDeleted);

		if (existingAmenity)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_EXISTED, request.Name);
		}

		var amenity = new Amenity
		{
			Name = request.Name,
			Description = request.Description,
			AmenityType = request.AmenityType,
		};


		_context.Amenities.Add(amenity);

		//_elasticSearchService.

		await _context.SaveChangesAsync(cancellationToken);

		return BuildMultilingualResult(result, amenity.Id.ToString(), Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
}

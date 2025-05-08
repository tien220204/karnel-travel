using AutoMapper;
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.Hotel.Models.Requests;
using KarnelTravel.Application.Features.Hotels.Models.Dtos;
using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Domain.Enums.Hotels;
using KarnelTravel.Share.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KarnelTravel.Application.Feature.Hotels;
public class CreateHotelCommand : CreateHotelRequest, IRequest<AppActionResultData<string>>
{
}

public class CreateHotelCommandHandler : BaseHandler, IRequestHandler<CreateHotelCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IElasticSearchService _elasticSearchService;
	private readonly IMapper _mapper;

	public CreateHotelCommandHandler(IApplicationDbContext context, IElasticSearchService elasticSearchService, IMapper mapper)
	{
		_context = context;
		_elasticSearchService = elasticSearchService;
		_mapper = mapper;
	}

	public async Task<AppActionResultData<string>> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();
		
		var country = await _context.Countries.FirstOrDefaultAsync(c => c.Code == request.CountryCode);

		if (country is null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, nameof(request.CountryCode));
		}

		var province = await _context.Provinces.Include(p => p.Districts).ThenInclude(d => d.Wards).FirstOrDefaultAsync(c => c.Code == request.ProvinceCode);

		if (province is null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, nameof(request.ProvinceCode));
		}

		var district = province.Districts.FirstOrDefault(c => c.Code == request.DistrictCode);

		if (district is null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, nameof(request.DistrictCode));
		}

		var ward = district.Wards.FirstOrDefault(c => c.Code == request.WardCode);

		if (ward is null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, nameof(request.WardCode));
		}

		//check for is defined enum type : PaymentType
		if (!Enum.IsDefined(typeof(PaymentType), request.PaymentType) || !Enum.TryParse(typeof(PaymentType), request.PaymentType.ToString(), out var paymentType))
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, nameof(request.PaymentType));
		}
;

		//check for is defined enum type : ServedMeals
		foreach (var meal in request.ServedMeals)
		{
			if (!Enum.IsDefined(typeof(ServedMeal), meal) || !Enum.TryParse(typeof(ServedMeal), meal.ToString(), out var servedMeal))
			{
				return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, nameof(meal));
			};
		}

		//convert policy request into obj
		var hotelPolicies = request.HotelPolicies.Select(p => new HotelPolicy
		{
			Type = p.Type,
			Description = p.Description
		}).ToList();

		//convert image request into obj
		var hotelImages = request.HotelImages.Select(i => new HotelImage
		{
			Name = i.Name,
			Url = i.Url,
			IsAvatar = i.IsAvatar
		}).ToList();

		//list id from amenity list request
		var requestAmenityIds = request.HotelAmenities.Select(a => a.AmenityId).Distinct().ToList();

		//find available amenities with request amenity ids and hotel's one existing in db 
		var amenitiesList = await _context.Amenities.Where(a => !a.IsDeleted && requestAmenityIds.Contains(a.Id) && a.AmenityType == Domain.Enums.MasterData.AmenityType.Hotel).ToListAsync();

		//check if all amenities in request are existed in db
		if (requestAmenityIds.Count != amenitiesList.Count)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, nameof(request.HotelAmenities));
		}

		//add to list
		var hotelAmenities = amenitiesList.Select(a => new HotelAmenity
		{
			AmenityId = a.Id,
			Amenity = a
		}).ToList();



		//create new room from request => add into list
		var hotelRooms = request.HotelRooms.Select(r => new HotelRoom
		{
			Code = r.Code,
			Description = r.Description,
			Capacity = r.Capacity,
			PricePerHour = r.PricePerHour
		}).ToList();

		//style id list in request
		var requestStyleIds = request.HotelStyles.Select(s => s.StyleId).Distinct().ToList();

		//style id list in db
		var stylesList = await _context.Style.Where(s => !s.IsDeleted && requestStyleIds.Contains(s.Id)).ToListAsync();

		//check if all styles in request are existed in db
		if (requestStyleIds.Count != stylesList.Count)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, nameof(request.HotelStyles));
		}

		//add to list
		var hotelStyles = stylesList.Select(s => new HotelStyle
		{
			StyleId = s.Id,
			Style = s
		}).ToList();


		var newHotel = new Hotel()
		{
			Name = request.Name,
			WardCode = request.WardCode,
			ProvinceCode = request.ProvinceCode,
			DistrictCode = request.DistrictCode,
			PaymentTypes = request.PaymentType,
			ServedMeals = request.ServedMeals,
			HotelPolicies = hotelPolicies,
			HotelImages = hotelImages,
			HotelAmenities = hotelAmenities,
			HotelRooms = hotelRooms,
			HotelStyles = hotelStyles
		};

		_context.Hotels.Add(newHotel);

		await _context.SaveChangesAsync(cancellationToken);


		//handle by event handler later
		var hotelElasticDto = _mapper.Map<HotelDto>(newHotel);
		await _elasticSearchService.CreateIndexIfNotExisted(nameof(Hotel));
		await _elasticSearchService.AddOrUpdate(hotelElasticDto, nameof(Hotel));


		return BuildMultilingualResult(result, newHotel.Id.ToString(), Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
}
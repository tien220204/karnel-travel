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
using Share.Common.Extensions;
using ZiggyCreatures.Caching.Fusion;


namespace KarnelTravel.Application.Features.Hotels.Commands;
public class UpdateHotelCommand : UpdateHotelRequest, IRequest<AppActionResultData<string>>
{
}

public class UpdateHotelCommandHandler : BaseHandler, IRequestHandler<UpdateHotelCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IFusionCache _fusionCache;
	private readonly IElasticSearchService _elasticSearchService;
	private readonly IMapper _mapper;
	public UpdateHotelCommandHandler(IApplicationDbContext context, IElasticSearchService elasticSearchService, IMapper mapper, IFusionCache fusionCache)
	{
		_context = context;
		_elasticSearchService = elasticSearchService;
		_mapper = mapper;
		_fusionCache = fusionCache;
	}
	
	public async Task<AppActionResultData<string>> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();

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

		var hotel = _context.Hotels.AsNoTracking().FirstOrDefault(x => x.Id == request.HotelId && !x.IsDeleted);

		if (hotel is null)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, request.HotelId);
		}

		//delete old policy list
		var oldPolicyList = hotel.HotelPolicies.ForEach(p => p.IsDeleted = true);

		//delete old image list
		var oldImageList = hotel.HotelImages.ForEach(i => i.IsDeleted = true);

		//delete old amenity list
		var oldAmenityList = hotel.HotelAmenities.ForEach(a => a.IsDeleted = true);

		//delete old style list
		var oldStyleList = hotel.HotelStyles.ForEach(s => s.IsDeleted = true);


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
			}
			;
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

		hotel.Name = request.Name;
		hotel.WardCode = request.WardCode;
		hotel.ProvinceCode = request.ProvinceCode;
		hotel.DistrictCode = request.DistrictCode;
		hotel.PaymentTypes = request.PaymentType;
		hotel.ServedMeals = request.ServedMeals;
		hotel.HotelPolicies = hotelPolicies;
		hotel.HotelImages = hotelImages;
		hotel.HotelAmenities = hotelAmenities;
		hotel.HotelStyles = hotelStyles;


		_context.Hotels.Update(hotel);
		await _context.SaveChangesAsync(cancellationToken);

		var hotelDto =  _mapper.Map<HotelDto>(hotel);
		await _elasticSearchService.CreateIndexIfNotExisted(nameof(Hotel));
		await _elasticSearchService.AddOrUpdate(hotelDto, nameof(Hotel));

		//await _fusionCache.RemoveAsync(CacheKeys.ALL_PRODUCT_CATEGORY);

		return BuildMultilingualResult(result, hotel.Id.ToString(), Resources.INF_MSG_SUCCESSFULLY);
	}
}
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.Hotel.Models.Requests;
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

	public CreateHotelCommandHandler(IApplicationDbContext context, IElasticSearchService elasticSearchService)
	{
		_context = context;
		_elasticSearchService = elasticSearchService;
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

		//initial policy list
		var hotelPolicies = new List<HotelPolicy>();

		//convert policy request into obj
		foreach (var policy in request.HotelPolicies)
		{
			var hotelPolicy = new HotelPolicy
			{
				Type = policy.Type,
				Description = policy.Description
			};

			hotelPolicies.Add(hotelPolicy);
		}

		//initial policy list
		var hotelImages = new List<HotelImage>();

		//convert image request into obj
		foreach (var image in request.HotelImages)
		{
			var hotelImage = new HotelImage
			{
				Name = image.Name,
				Url = image.Url,
				IsAvatar = image.IsAvatar,
			};

			hotelImages.Add(hotelImage);
		}

		//initial amenity list
		var hotelAmenities = new List<HotelAmenity>();

		foreach(var req in request.HotelAmenities)
		{
			var amenity = await _context.Amenities.FirstOrDefaultAsync(a => !a.IsDeleted && a.Id == req.AmenityId && a.AmenityType == Domain.Enums.MasterData.AmenityType.Hotel);

			if (amenity == null)
			{
				return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, nameof(req.AmenityId));
			}

			//use existing amenity for hotel amenity
			hotelAmenities.Add(new HotelAmenity
			{
				AmenityId = amenity.Id,
				Amenity = amenity 
			});
		}

		//inital hotel room list
		var hotelRooms = new List<HotelRoom>();

		//create new room from request => add into list
		foreach (var req in request.HotelRooms)
		{
			var hotelRoom = new HotelRoom
			{
				Code = req.Code,
				Description = req.Description,
				Capacity = req.Capacity,
				PricePerHour = req.PricePerHour
			};

			hotelRooms.Add(hotelRoom);
		}

		var hotelStyles = new List<HotelStyle>();

		foreach (var req in request.HotelStyles)
		{
			var style = await _context.Style.FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == req.StyleId);

			if (style == null)
			{
				return BuildMultilingualError(result, Resources.ERR_MSG_DATA_WITH_ID_NOT_FOUND, nameof(req.StyleId));
			}

			//use existing style for hotel style
			hotelStyles.Add(new HotelStyle
			{
				StyleId = style.Id,
				Style = style
			});
		}

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

		await _elasticSearchService.CreateIndexIfNotExisted(nameof(Hotel));
		await _elasticSearchService.AddOrUpdate(newHotel, nameof(Hotel));

		return BuildMultilingualResult(result, newHotel.Id.ToString(), Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
}
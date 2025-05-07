using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.Hotel.Models.Requests;
using KarnelTravel.Share.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace KarnelTravel.Application.Features.Hotels.Commands;
public class UpdateHotelCommand : UpdateHotelRequest, IRequest<AppActionResultData<string>>
{
}

public class UpdateHotelCommandHandler : BaseHandler, IRequestHandler<UpdateHotelCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IFusionCache _fusionCache;
	public UpdateHotelCommandHandler(IApplicationDbContext context, IFusionCache fusionCache)
	{
		_context = context;
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

		hotel.Name = request.Name;
		hotel.WardCode = request.WardCode;
		hotel.ProvinceCode = request.ProvinceCode;
		hotel.DistrictCode = request.DistrictCode;
		hotel.CountryCode = request.CountryCode;
		hotel.HotelClass = request.HotelClass;
		//hotel.HotelStyles = request.HotelStyles;
		hotel.Latitude = request.Latitude;
		hotel.Longitude = request.Longitude;






		_context.Hotels.Update(hotel);
		await _context.SaveChangesAsync(cancellationToken);


		//await _fusionCache.RemoveAsync(CacheKeys.ALL_PRODUCT_CATEGORY);

		return BuildMultilingualResult(result, hotel.Id.ToString(), Resources.INF_MSG_SUCCESSFULLY);
	}
}
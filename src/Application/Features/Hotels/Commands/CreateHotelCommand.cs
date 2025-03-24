using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.Hotel.Models.Requests;
using KarnelTravel.Domain.Entities.Features.Hotels;
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

	public CreateHotelCommandHandler(IApplicationDbContext context)
	{
		_context = context;
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

		

		var newHotel = new Hotel()
		{
			Name = request.Name,
			WardCode = request.WardCode,
			ProvinceCode = request.ProvinceCode,
			DistrictCode = request.DistrictCode, 
		}
		;



		_context.Hotels.Add(newHotel);
		await _context.SaveChangesAsync(cancellationToken);

		return BuildMultilingualResult(result, newHotel.Id.ToString(), Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
}
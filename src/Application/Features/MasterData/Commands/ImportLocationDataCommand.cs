
using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Application.Features.MasterData.Dtos;
using KarnelTravel.Domain.Entities.Features.MasterData;
using KarnelTravel.Share.CloudinaryService.Interfaces;
using KarnelTravel.Share.Common.Helpers;
using KarnelTravel.Share.Localization;
using KarnelTravel.Share.Utilities.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Share.Common.Extensions;

namespace KarnelTravel.Application.Features.MasterData.Commands;
public record ImportLocationDataCommand : IRequest<AppActionResultData<string>>
{
	public IFormFile File { get; set; }
}
public class ImportLocationDataCommandHandler : BaseHandler, IRequestHandler<ImportLocationDataCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICloudinaryUploadService _cloudinaryUploadService;
	public ImportLocationDataCommandHandler(IApplicationDbContext context, ICloudinaryUploadService cloudinaryUploadService)
	{
		_context = context;
		_cloudinaryUploadService = cloudinaryUploadService;
	}

	public async Task<AppActionResultData<string>> Handle(ImportLocationDataCommand command, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();

		if (command.File is not null)
		{
			var importResult = await ImportProductLocationAsync(command, cancellationToken);
			if (importResult.IsSuccess)
			{
				return result.BuildResult("thanh cong");
			}
		}

		return result.BuildResult("abc");
	}

	/// Import Category  asynchronously
	/// </summary>
	/// <param name="request">request</param>
	/// <returns></returns>
	public async Task<AppActionResultData<string>> ImportProductLocationAsync(ImportLocationDataCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();

		var existedCountry = _context.Countries.ToList();
		if (existedCountry.Count() > 0)
		{
			_context.Countries.RemoveRange(existedCountry);
		}

		var existedProvinces = _context.Provinces.ToList();
		if (existedProvinces.Count() > 0)
		{
			_context.Provinces.RemoveRange(existedProvinces);
		}

		var existedDistricts = _context.Districts.ToList();
		if (existedDistricts.Count() > 0)
		{
			_context.Districts.RemoveRange(existedDistricts);
		}

		var existedWards = _context.Wards.ToList();
		if (existedWards.Count() > 0)
		{
			_context.Wards.RemoveRange(existedWards);
		}

		using var stream = new MemoryStream();
		await request.File.CopyToAsync(stream);
		var data = ExcelReader.ReadExcelWorksheet(stream, rowStart: 1);
		var locationRecord = ExcelHelper.ConvertDataFromFile<LocationImportFileDto>(data);
		if (locationRecord.IsNullOrEmpty())
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_FILE_IS_EMPTY);
		}

		var newCountries = locationRecord.GroupBy(x => new { x.CountryName, x.CountryCode }).Select(x => new Country
		{
			Name = x.Key.CountryName,
			Code = x.Key.CountryCode
		}).ToList();

		var newProvinces = locationRecord.GroupBy(x => new { x.ProvinceName, x.ProvinceCode }).Select(x => new Province
		{
			Name = x.Key.ProvinceName,
			Code = x.Key.ProvinceCode,
		}).ToList();

		var newDistricts = locationRecord.GroupBy(x => new { x.DistrictName, x.DistrictCode, x.ProvinceCode }).Select(x => new District
		{
			Name = x.Key.DistrictName,
			Code = x.Key.DistrictCode,
			ParentCode = x.Key.ProvinceCode,
		}).ToList();

		var newWards = locationRecord.GroupBy(x => new { x.WardName, x.WardCode, x.DistrictCode }).Select(x => new Ward
		{
			Name = x.Key.WardName,
			Code = x.Key.WardCode,
			ParentCode = x.Key.DistrictCode,
		}).ToList();


		await _context.Countries.AddRangeAsync(newCountries);
		await _context.Provinces.AddRangeAsync(newProvinces);
		await _context.Districts.AddRangeAsync(newDistricts);
		await _context.Wards.AddRangeAsync(newWards);

		await _context.SaveChangesAsync(cancellationToken);

		return result;
	}
}

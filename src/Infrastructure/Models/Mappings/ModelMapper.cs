using AutoMapper;
using KarnelTravel.Application.Features.Hotels.Models.Dtos;
using KarnelTravel.Application.Features.MasterData.Dtos;
using KarnelTravel.Domain.Entities.Features.Hotel;
using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Domain.Entities.Features.MasterData;

namespace KarnelTravel.Infrastructure.Models.Mappings;
public class ModelMapper
{
	public static void MappingDto(IMapperConfigurationExpression config)
	{
		config.DisableConstructorMapping();
		ConfigureHotelDtoMapping(config);

		ConfigureMasterDataDtoMapping(config);
	}

	private static void ConfigureHotelDtoMapping(IMapperConfigurationExpression config)
	{
		config.CreateMap<Hotel, HotelDto>(MemberList.Destination);

		config.CreateMap<HotelImage, HotelImageDto>(MemberList.Destination);

		config.CreateMap<HotelPolicy, HotelPolicyDto>(MemberList.Destination);

		config.CreateMap<HotelAmenity, HotelAmenityDto>(MemberList.Destination);

		config.CreateMap<HotelRating, HotelRatingDto>(MemberList.Destination);

		config.CreateMap<HotelRoom, HotelRoomDto>(MemberList.Destination);

		config.CreateMap<HotelReview, HotelReviewDto>(MemberList.Destination);

		config.CreateMap<HotelStyle, HotelStyleDto>(MemberList.Destination);

		config.CreateMap<Style, StyleDto>(MemberList.Destination);
	}


	private static void ConfigureMasterDataDtoMapping(IMapperConfigurationExpression config)
	{
		config.CreateMap<Country, CountryDto>(MemberList.Destination);

		config.CreateMap<Province, ProvinceDto>(MemberList.Destination);

		config.CreateMap<District, DistrictDto>(MemberList.Destination);

		config.CreateMap<Ward, WardDto>(MemberList.Destination);

		config.CreateMap<Amenity, AmenityDto>(MemberList.Destination);

	}
}

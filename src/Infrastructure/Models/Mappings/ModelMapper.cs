using AutoMapper;

namespace KarnelTravel.Infrastructure.Models.Mappings;
public class ModelMapper
{
	public static void MappingDto(IMapperConfigurationExpression config)
	{
		config.DisableConstructorMapping();
		ConfigureProductsDtoMapping(config);
	}

	private static void ConfigureProductsDtoMapping(IMapperConfigurationExpression config)
	{
		//config.CreateMap<ProductCategory, ProductCategoryDto>(MemberList.Destination);

		//config.CreateMap<ProductCategory, ProductCategoryInfoDto>(MemberList.Destination);

		//config.CreateMap<Product, ProductDto>(MemberList.Destination)
		//	  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => AutoMapperHelper.GuidToStringConverter(src.Id)));

		//config.CreateMap<ProductUnit, ProductUnitDto>(MemberList.Destination);
		//config.CreateMap<ProductImage, ProductImageDto>(MemberList.Destination)
		//	  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => AutoMapperHelper.GuidToStringConverter(src.Id)));

		//config.CreateMap<ProductSpecifications, ProductSpecificationsDto>(MemberList.Destination);

		//config.CreateMap<ProductProperty, ProductPropertyDto>(MemberList.Destination);

		//config.CreateMap<ProductPropertyDetail, ProductPropertyDetailDto>(MemberList.Destination)
		//	.ForMember(dest => dest.Id, opt => opt.MapFrom(src => AutoMapperHelper.GuidToStringConverter(src.Id)))
		//	.ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => AutoMapperHelper.GuidToStringConverter(src.ProductId)));

		//config.CreateMap<Document, DocumentDto>(MemberList.Destination);

		//config.CreateMap<ProductReward, ProductRewardDto>(MemberList.Destination)
		//	  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => AutoMapperHelper.GuidToStringConverter(src.Id)))
		//	  .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => AutoMapperHelper.GuidToStringConverter(src.ProductId)));
	}
}

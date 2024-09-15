using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using KarnelTravel.Domain.Entities.Features.Hotels;

namespace KarnelTravel.Infrastructure.Configurations.Features.Hotels;
public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
	public void Configure(EntityTypeBuilder<Hotel> builder)
	{
		builder.HasKey(p => p.Id);

		//builder.Property(p => p.Name).IsRequired().HasMaxLength(255);
		//builder.Property(p => p.Code).IsRequired().HasMaxLength(50);
		//builder.Property(p => p.CommonCode).HasMaxLength(50);

		builder.HasOne(dlo => dlo.Country).WithMany().HasForeignKey(dlo => dlo.CountryCode).HasPrincipalKey(c => c.Code);
		builder.HasOne(dlo => dlo.Province).WithMany().HasForeignKey(dlo => dlo.ProvinceCode).HasPrincipalKey(p => p.Code);
		builder.HasOne(dlo => dlo.District).WithMany().HasForeignKey(dlo => dlo.DistrictCode).HasPrincipalKey(d => d.Code);
		builder.HasOne(dlo => dlo.Ward).WithMany().HasForeignKey(dlo => dlo.WardCode).HasPrincipalKey(w => w.Code);
		builder.HasMany(p => p.HotelImages).WithOne(pi => pi.Hotel).HasForeignKey(po => po.HotelId);
		builder.HasMany(p => p.HotelAmenities).WithOne(pi => pi.Hotel).HasForeignKey(po => po.HotelId);
		builder.HasMany(p => p.HotelRooms).WithOne(pi => pi.Hotel).HasForeignKey(po => po.HotelId);
		builder.HasMany(p => p.HotelReviews).WithOne(pi => pi.Hotel).HasForeignKey(po => po.HotelId);
		builder.HasMany(p => p.HotelStyles).WithOne(pi => pi.Hotel).HasForeignKey(po => po.HotelId);
		
		
	}
}

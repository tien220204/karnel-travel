using KarnelTravel.Domain.Entities.Features.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KarnelTravel.Infrastructure.Configurations.Features.MasterData;
public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
{
	public void Configure(EntityTypeBuilder<Province> builder)
	{
		builder.Property(p => p.Id).ValueGeneratedOnAdd();
		builder.Property(p => p.Code).IsRequired();
		builder.HasIndex(p => p.Code).IsUnique();
		builder.Property(c => c.Id).ValueGeneratedOnAdd();
		builder.HasMany(p => p.Districts).WithOne(d => d.Province).HasForeignKey(d => d.ParentCode).HasPrincipalKey(p => p.Code);
		builder.HasOne(p => p.Country).WithMany(c => c.Provinces).HasForeignKey(p => p.ParentCode).HasPrincipalKey(c => c.Code);
	}
}




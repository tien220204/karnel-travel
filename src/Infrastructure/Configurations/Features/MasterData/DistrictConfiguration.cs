using KarnelTravel.Domain.Entities.Features.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KarnelTravel.Infrastructure.Configurations.Features.MasterData;
public class DistrictConfiguration : IEntityTypeConfiguration<District>
{
	public void Configure(EntityTypeBuilder<District> builder)
	{
		builder.Property(d => d.Id).ValueGeneratedOnAdd();
		builder.Property(d => d.Code).IsRequired();
		builder.HasIndex(d => d.Code).IsUnique();
		builder.HasMany(d => d.Wards).WithOne(w => w.District).HasForeignKey(w => w.ParentCode).HasPrincipalKey(d => d.Code);
		builder.HasOne(d => d.Province).WithMany(p => p.Districts).HasForeignKey(d => d.ParentCode).HasPrincipalKey(p => p.Code);
	}
}
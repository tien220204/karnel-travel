using KarnelTravel.Domain.Entities.Features.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KarnelTravel.Infrastructure.Configurations.Features.MasterData;
public class WardConfiguration : IEntityTypeConfiguration<Ward>
{
	public void Configure(EntityTypeBuilder<Ward> builder)
	{
		builder.Property(w => w.Id).ValueGeneratedOnAdd();
		builder.Property(w => w.Code).IsRequired();
		builder.HasIndex(w => w.Code).IsUnique();
		builder.HasOne(w => w.District).WithMany(d => d.Wards).HasForeignKey(w => w.ParentCode).HasPrincipalKey(d => d.Code);

	}
}

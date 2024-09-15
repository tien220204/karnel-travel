using KarnelTravel.Domain.Entities.Features.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KarnelTravel.Infrastructure.Configurations.Features.MasterData;
public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
	public void Configure(EntityTypeBuilder<Country> builder)
	{

		builder.Property(c => c.Id).ValueGeneratedOnAdd();
		builder.Property(c => c.Code).IsRequired();
		builder.HasIndex(c => c.Code).IsUnique();
		builder.HasMany(c => c.Provinces).WithOne(p => p.Country).HasForeignKey(ppd => ppd.ParentCode).HasPrincipalKey(c => c.Code);
	}
}

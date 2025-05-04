using KarnelTravel.Domain.Entities.Features.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace KarnelTravel.Infrastructure.Configurations.Features.Users;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
	public void Configure(EntityTypeBuilder<ApplicationUser> builder)
	{
		// Each User can have many UserClaims
		//builder.HasMany(e => e.Claims)
		//	.WithOne(e => e.User)
		//	.HasForeignKey(uc => uc.UserId)
		//	.IsRequired();

		// Each User can have many entries in the UserRole join table
		//builder.HasMany(e => e.UserRoles)
		//	.WithOne(e => e.User)
		//	.HasForeignKey(ur => ur.UserId)
		//	.IsRequired();

		builder.Property(e => e.FullName)
			.IsRequired()
			.HasMaxLength(550);

		builder.Property(e => e.Address)
		   .HasMaxLength(500);

		builder.Property(e => e.DistrictName)
			.HasMaxLength(255);

		builder.Property(e => e.WardName)
			.HasMaxLength(255);

		builder.Property(e => e.ProvinceName)
			.HasMaxLength(255);

		

	}
}
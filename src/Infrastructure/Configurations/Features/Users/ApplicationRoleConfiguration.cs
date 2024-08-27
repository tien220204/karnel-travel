﻿using KarnelTravel.Domain.Entities.Features.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace KarnelTravel.Infrastructure.Configurations.Features.Users;


public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
	public void Configure(EntityTypeBuilder<ApplicationRole> builder)
	{
		// Each Role can have many entries in the UserRole join table
		builder.HasMany(e => e.UserRoles)
			.WithOne(e => e.Role)
			.HasForeignKey(ur => ur.RoleId)
			.IsRequired();

		// Each Role can have many associated RoleClaims
		builder.HasMany(e => e.RoleClaims)
			.WithOne(e => e.Role)
			.HasForeignKey(rc => rc.RoleId)
			.IsRequired();

		builder.Property(e => e.IsActive)
			.HasDefaultValue(true);

		builder.HasQueryFilter(p => p.IsActive);
	}
}
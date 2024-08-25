using KarnelTravel.Domain.Entities.Features.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using KarnelTravel.Domain.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using KarnelTravel.Application.Common.Interfaces;

namespace KarnelTravel.Infrastructure.Data;

public static class InitialiserExtensions
{
	public static async Task InitialiseDatabaseAsync(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();

		var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

		await initialiser.InitialiseAsync();
		await initialiser.InitialisePersistedGrantDbAsync();
		// await initialiser.SeedAsync();
	}
}

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IApplicationDbContext, IDataProtectionKeyContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

	//implement IDataProtectionKeyContext
	public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
	public DbSet<ApplicationRole> AspNetRoles => Set<ApplicationRole>();
	public DbSet<ApplicationUser> AspNetUsers => Set<ApplicationUser>();
	public DbSet<ApplicationUserClaim> AspNetUserClaims => Set<ApplicationUserClaim>();
	public DbSet<ApplicationRoleClaim> AspNetRoleClaims => Set<ApplicationRoleClaim>();
	public DbSet<ApplicationUserRole> AspNetUserRoles => Set<ApplicationUserRole>();

	//implement IApplicationDbContext
	#region product
	//public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
	#endregion product


	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
	{
		foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedBy = "System";
					entry.Entity.Created = DateTime.Now;
					break;

				case EntityState.Modified:
					entry.Entity.LastModifiedBy = "System";
					entry.Entity.LastModified = DateTime.Now;
					break;
			}
		}

		var result = await base.SaveChangesAsync(cancellationToken);
		return result;
	}
}

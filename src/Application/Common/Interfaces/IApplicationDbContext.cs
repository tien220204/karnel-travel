using KarnelTravel.Domain.Entities.Features.MasterData;
using Microsoft.EntityFrameworkCore;

namespace KarnelTravel.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	//DbSet<ProductCategory> ProductCategories { get; }
	DbSet<Country> Countries { get; }
	DbSet<Province> Provinces { get; }
	DbSet<District> Districts { get; }
	DbSet<Ward> Wards { get; }
	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

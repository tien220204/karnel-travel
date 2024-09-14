using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Domain.Entities.Features.MasterData;
using Microsoft.EntityFrameworkCore;

namespace KarnelTravel.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	//DbSet<ProductCategory> ProductCategories { get; }
	//hotel
	DbSet<Hotel> Hotels { get; }
	DbSet<HotelAmenity> HotelAmenities { get; }
	DbSet<HotelPolicy> HotelPolicies { get; }
	DbSet<HotelReview> HotelReviews { get; }
	DbSet<HotelRoom> HotelRooms { get; }
	DbSet<HotelImage> HotelImages { get; }
	DbSet<HotelPropertyType> HotelPropertyTypes { get; }
	DbSet<HotelStyle> HotelStyles { get; }
	DbSet<Style> Style { get; }
	

	//master data
	DbSet<Country> Countries { get; }
	DbSet<Province> Provinces { get; }
	DbSet<District> Districts { get; }
	DbSet<Ward> Wards { get; }
	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

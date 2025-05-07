using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Entities.Features.Flights;
using KarnelTravel.Domain.Entities.Features.Hotel;
using KarnelTravel.Domain.Entities.Features.Hotels;
using KarnelTravel.Domain.Entities.Features.MasterData;
using KarnelTravel.Domain.Entities.Features.Users;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Metadata;

namespace KarnelTravel.Infrastructure.Data;


public class ApplicationDbContext :
	DbContext,
	IApplicationDbContext,
	IDataProtectionKeyContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

	public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
	public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();


	#region hotel
	public DbSet<Hotel> Hotels => Set<Hotel>();
	public DbSet<HotelAmenity> HotelAmenities => Set<HotelAmenity>();
	public DbSet<HotelPolicy> HotelPolicies => Set<HotelPolicy>();
	public DbSet<HotelReview> HotelReviews => Set<HotelReview>();
	public DbSet<HotelRoom> HotelRooms => Set<HotelRoom>();
	public DbSet<HotelImage> HotelImages => Set<HotelImage>();
	public DbSet<HotelPropertyType> HotelPropertyTypes => Set<HotelPropertyType>();
	public DbSet<HotelStyle> HotelStyles => Set<HotelStyle>();
	public DbSet<Style> Style => Set<Style>();
	public DbSet<HotelRating> HotelRatings => Set<HotelRating>();
	#endregion hotels

	#region document
	//public DbSet<Document> Documents => Set<Document>();

	#endregion document

	#region masterdata
	public DbSet<Country> Countries => Set<Country>();
	public DbSet<Province> Provinces => Set<Province>();
	public DbSet<District> Districts => Set<District>();
	public DbSet<Ward> Wards => Set<Ward>();
	public DbSet<Amenity> Amenities => Set<Amenity>();


	#endregion masterdata

	#region flight
	public DbSet<Flight> Flights => Set<Flight>();
	public DbSet<FlightTicket> FlightsTickets => Set<FlightTicket>();
	public DbSet<FlightExtension> FlightsExtensions => Set<FlightExtension>();
	public DbSet<Airport> Airports => Set<Airport>();
	public DbSet<Airline> Airlines => Set<Airline>();
	#endregion

	#region booking
	public DbSet<BookingDetail> BookingDetail => Set<BookingDetail>();
	#endregion

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

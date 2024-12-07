using KarnelTravel.Domain.Entities.Features.Hotels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarnelTravel.Domain.Entities.Features.Flights;

namespace KarnelTravel.Infrastructure.Configurations.Features.Flights;
public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
	public void Configure(EntityTypeBuilder<Flight> builder)
	{
		builder.HasKey(x => x.Id);

		builder.HasOne(f => f.Airline).WithMany(a => a.Flights).HasForeignKey(f => f.AirlineId);
		builder.HasOne(f => f.Airport).WithMany(a => a.Flights).HasForeignKey(f => f.ArrivalAirportId);
		builder.HasOne(f => f.Airport).WithMany(a => a.Flights).HasForeignKey(f => f.DepartureAirportId);
		builder.HasMany(f => f.FlightTickets).WithOne(ft => ft.Flight).HasForeignKey(ft => ft.FLightId);
		builder.HasMany(f => f.FlightExtensions).WithOne(fe => fe.Flight).HasForeignKey(fe => fe.FlightId);
	}
}



using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace KarnelTravel.Infrastructure.Configurations.Features.BookingDetails;
public class FlightTicketBookingConfiguration : IEntityTypeConfiguration<FlightTicketBooking>
{
	public void Configure(EntityTypeBuilder<FlightTicketBooking> builder)
	{
		builder.HasKey(x => x.Id);

		builder.HasOne(ftb => ftb.FlightTicket).WithMany().HasForeignKey(ftb => ftb.FLightTicketId).HasPrincipalKey(f => f.Id);
	}
}

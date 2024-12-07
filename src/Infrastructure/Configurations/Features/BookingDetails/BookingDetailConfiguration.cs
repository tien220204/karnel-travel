using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KarnelTravel.Infrastructure.Configurations.Features.BookingDetails;
public class BookingDetailConfiguration : IEntityTypeConfiguration<BookingDetail>
{
	public void Configure(EntityTypeBuilder<BookingDetail> builder)
	{
		builder.HasKey(b => b.Id);

		builder.HasMany(b => b.FlightTicketBookings).WithOne(ftb => ftb.BookingDetail).HasForeignKey(ftb => ftb.BookingDetailId);
		builder.HasMany(b => b.HotelRoomBooking).WithOne(ftb => ftb.BookingDetail).HasForeignKey(ftb => ftb.BookingDetailId);

	}
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace KarnelTravel.Infrastructure.Configurations.Features.BookingDetails;
public class HotelRoomBookingConfiguration : IEntityTypeConfiguration<HotelRoomBooking>
{
	public void Configure(EntityTypeBuilder<HotelRoomBooking> builder)
	{
		builder.HasKey(x => x.Id);

		builder.HasOne(ftb => ftb.HotelRoom).WithMany().HasForeignKey(ftb => ftb.HotelRoomId).HasPrincipalKey(f => f.Id);
	}
}
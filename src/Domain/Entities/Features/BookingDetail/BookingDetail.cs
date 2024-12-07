using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Enums.BookingDetails;

public class BookingDetail : BaseAuditableEntity<long>
{
	public ICollection<FlightTicketBooking> FlightTicketBookings { get; set; }
	public ICollection<HotelRoomBooking> HotelRoomBooking { get; set; }
	public BookingDetailType BookingDetailType { get; set; }
}

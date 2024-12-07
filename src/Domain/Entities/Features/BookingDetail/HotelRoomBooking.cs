using KarnelTravel.Domain.Common;
using KarnelTravel.Domain.Entities.Features.Hotels;

public class HotelRoomBooking : BaseAuditableEntity<long>
{
    public long HotelRoomPrice { get; set; }
    public long HotelRoomId { get; set; }
    public long BookingDetailId { get; set; }
    public HotelRoom HotelRoom { get; set; }
    public BookingDetail BookingDetail { get; set; }
}

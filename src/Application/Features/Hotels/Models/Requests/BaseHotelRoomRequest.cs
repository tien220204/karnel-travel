using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.Hotels.Models.Requests;
public class BaseHotelRoomRequest
{
	public string Code { get; set; }
	public string Description { get; set; }
	public long Capacity { get; set; }
	public long PricePerHour { get; set; }
}

public class CreateHotelRoomRequest : BaseHotelRoomRequest
{
}
public class UpdateHotelRoomRequest : BaseHotelRoomRequest
{
	public long Id { get; set; }
}
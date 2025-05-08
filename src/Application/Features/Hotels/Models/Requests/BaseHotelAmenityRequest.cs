using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.Hotels.Models.Requests;
public class BaseHotelAmenityRequest
{
	public long AmenityId { get; set; }
}

public class CreateHotelAmenityRequest : BaseHotelAmenityRequest
{

}

public class UpdateHotelAmenityRequest : BaseHotelAmenityRequest
{
	public long Id { get; set; }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.Hotels.Models.Requests;
public class BaseHotelStyleRequest
{
	public long StyleId { get; set; }
}

public class CreateHotelStyleRequest : BaseHotelStyleRequest
{
}

public class UpdateHotelStyleRequest : BaseHotelStyleRequest
{
	public long Id { get; set; }
}
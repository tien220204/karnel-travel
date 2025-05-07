using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.Hotels.Models.Requests;
public class BaseHotelImageRequest 
{
	public string Name { get; set; }
	public string Url { get; set; }
	public bool IsAvatar { get; set; }
}

public class CreateHotelImageRequest : BaseHotelImageRequest
{
	
}

public class UpdateHotelImageRequest : BaseHotelImageRequest
{
	public long Id { get; set; }
}


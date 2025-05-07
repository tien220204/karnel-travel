using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.Hotels.Models.Requests;
public class BaseHotelPolicyRequest
{
	public string Type { get; set; }
	public string Description { get; set; }
}

public class CreateHotelPolicyRequest : BaseHotelPolicyRequest
{

}

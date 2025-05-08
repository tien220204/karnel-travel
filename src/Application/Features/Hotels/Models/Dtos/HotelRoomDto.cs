using Share.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.Hotels.Models.Dtos;
public class HotelRoomDto : BaseAuditableEntityDto<long>
{
	public string Code { get; set; }
	public string Description { get; set; }
	public long Capacity { get; set; }
	public long PricePerHour { get; set; }
}

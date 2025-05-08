using KarnelTravel.Domain.Entities.Features.Hotel;
using Share.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.Hotels.Models.Dtos;
public class HotelStyleDto : BaseAuditableEntityDto<long>
{
	public long StyleId { get; set; }
	public StyleDto Style { get; set; }
}

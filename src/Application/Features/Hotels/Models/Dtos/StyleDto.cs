using KarnelTravel.Domain.Entities.Features.Hotels;
using Share.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.Hotels.Models.Dtos;
public class StyleDto : BaseAuditableEntityDto<long>
{
	public string Name { get; set; }
	public string Description { get; set; }
}

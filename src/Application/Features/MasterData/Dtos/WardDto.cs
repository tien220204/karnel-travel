using Share.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.MasterData.Dtos;
public class WardDto : BaseAuditableEntityDto<long>
{
	public string Name { get; set; }
	public string Code { get; set; }
	public string ParentCode { get; set; }
}

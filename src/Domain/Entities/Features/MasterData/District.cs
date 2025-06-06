﻿using KarnelTravel.Domain.Common;

namespace KarnelTravel.Domain.Entities.Features.MasterData;
public class District : BaseEntity<long>
{
	public string Name { get; set; }
	public string Code { get; set; }
	public string ParentCode { get; set; }
    public Province Province { get; set; }
    public ICollection<Ward> Wards { get; set; }
}
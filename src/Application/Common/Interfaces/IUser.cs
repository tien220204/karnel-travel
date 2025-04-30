using System.Security.Claims;

namespace KarnelTravel.Application.Common.Interfaces;

public interface IUser
{
	string? Id { get; }
	ClaimsPrincipal? User { get; } 
}

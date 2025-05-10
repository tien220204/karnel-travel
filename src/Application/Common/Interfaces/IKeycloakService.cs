using KarnelTravel.Application.Features.ApplicationUsers.Models.Dtos.Token;

namespace KarnelTravel.Application.Common.Interfaces;
public interface IKeycloakService
{

	Task<string> CreateUserAsync(string username, string password, string email, string firstName, string lastName);
	Task<bool> DeleteUserAsync(string keycloakUserId);
	Task<bool> AssignRoleAsync(string keycloakUserId, string role);
	Task<List<string>> GetUserRolesAsync(string keycloakUserId);
	Task<string> GetUserToken(string username, string password);


}

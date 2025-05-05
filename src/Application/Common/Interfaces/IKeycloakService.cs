using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Common.Interfaces;
public interface IKeycloakService
{

	Task<string> CreateUserAsync(string username, string password, string email, string firstName, string lastName);
	Task<bool> DeleteUserAsync(string keycloakUserId);
	Task<bool> AssignRoleAsync(string keycloakUserId, string role);
	Task<List<string>> GetUserRolesAsync(string keycloakUserId);
	
}

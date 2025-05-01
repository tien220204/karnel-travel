using KarnelTravel.Application.Common.Interfaces;
using Keycloak.Net.Models.Root;
using Keycloak.Net.Models.Users;
using Keycloak.Net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Keycloak.Net.Models.Roles;

namespace KarnelTravel.Infrastructure.Keycloak;
public class KeycloakService : IKeycloakService
{
	private readonly KeycloakClient _client;
	private readonly string _realm;
	private readonly string _clientId;

	public KeycloakService(IConfiguration config)
	{
		var url = config["Keycloak:ServerUrl"];
		var adminUser = config["Keycloak:AdminUser"];
		var adminPassword = config["Keycloak:AdminPassword"];
		_clientId = config["Keycloak:ClientId"];
		var clientSecret = config["Keycloak:ClientSecret"];
		_realm = config["Keycloak:Realm"];

		_client = new  KeycloakClient(
			url: url,      
			clientSecret: clientSecret    
		);
	}

	public async Task<string> CreateUserAsync(string username, string password)
	{
		var user = new User
		{
			UserName = username,
			//Email = email,
			Enabled = true,
			Credentials = new List<Credentials>
			{
				new Credentials
				{
					Type = "password",
					Value = password,
					Temporary = false
				}
			}
		};

		var success = await _client.CreateUserAsync(_realm, user);
		

		if (!success)
			return null;

		var users = await _client.GetUsersAsync(_realm, username:username);
		var createdUser = users?.FirstOrDefault(u => u.UserName == username);
		return createdUser?.Id;
	}

	public async Task<bool> DeleteUserAsync(string keycloakUserId)
	{
		return await _client.DeleteUserAsync(_realm, keycloakUserId);
	}

	public async Task<bool> AssignRoleAsync(string keycloakUserId, string roleName)
	{
		var user = await _client.GetUserAsync(_realm, keycloakUserId);
		if (user == null) return false;

		// Get available realm roles
		var allRoles = await _client.GetRolesAsync(_realm, _clientId );
		var targetRole = allRoles.FirstOrDefault(r => r.Name == roleName);

		if (targetRole == null) return false;

		// Assign role to user
		return await _client.AddClientRoleMappingsToUserAsync(_realm, keycloakUserId, _clientId, new[] { targetRole });
	}

	public async Task<List<string>> GetUserRolesAsync(string keycloakUserId)
	{
		var roles = await _client.GetRealmRoleMappingsForUserAsync(_realm, keycloakUserId);
		return roles?.Select(r => r.Name).ToList() ?? new List<string>();
	}
}

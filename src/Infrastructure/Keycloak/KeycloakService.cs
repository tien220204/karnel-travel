using KarnelTravel.Application.Common.Interfaces;
using Keycloak.Net;
using Keycloak.Net.Models.Clients;
using Keycloak.Net.Models.Users;
using Microsoft.Extensions.Configuration;

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

		_client = new KeycloakClient(
			url: url,
			clientSecret: clientSecret
		);
	}

	public async Task<string> CreateUserAsync(string username, string password, string email, string firstName, string lastName)
	{
		var user = new User
		{
			UserName = username,
			Email = email,
			Enabled = true,
			FirstName = firstName,
			LastName = lastName,
			Credentials = new List<Credentials>
			{
				new Credentials
				{
					Type = "password",
					Value = password
				}
			}
		};

		try
		{
			var userKeycloakId = await _client.CreateAndRetrieveUserIdAsync(_realm, user);

			return userKeycloakId;
		}
		catch (Flurl.Http.FlurlHttpException ex)
		{
			if (ex.Call.Response?.StatusCode == System.Net.HttpStatusCode.Conflict)
			{
				
				throw new ApplicationException("User already exists in keycloak.");
			}

			// Log lỗi chi tiết hoặc ném lại nếu là lỗi khác
			throw;
		}
		//var userKeycloakId = await _client.CreateAndRetrieveUserIdAsync(_realm, user);

		//return userKeycloakId;

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
		var allRoles = await _client.GetRolesAsync(_realm, _clientId);
		var targetRole = allRoles.FirstOrDefault(r => r.Name == roleName);

		if (targetRole == null) return false;

		// Assign role to user
		return await _client.AddClientRoleMappingsToUserAsync(_realm, keycloakUserId, _clientId, new[] { targetRole });
	}

	//private async Task<Client> GetClientAsync(string clientId)
	//{
	//	var clients = await _client.GetClientsAsync(_realm);

	//	//client uuid
	//	var client = clients.FirstOrDefault(c => c.ClientId == clientId);
	//	return client;

	//} 

	public async Task<List<string>> GetUserRolesAsync(string keycloakUserId)
	{
		var clientUuid = await GetClientUuidAsync(_clientId);
		if (string.IsNullOrEmpty(clientUuid))
			throw new Exception("Client not found in Keycloak");

		//var roles = await _client.GetRealmRoleMappingsForUserAsync(_realm, keycloakUserId);
		var roles = await _client.GetClientRoleMappingsForUserAsync(_realm, keycloakUserId, clientUuid);
		return roles?.Select(r => r.Name).ToList() ?? new List<string>();
	}

	private async Task<string> GetClientUuidAsync(string clientId)
	{
		var clients = await _client.GetClientsAsync(_realm);
		var client = clients.FirstOrDefault(c => c.ClientId == clientId);
		return client?.Id; 
	}

	
}

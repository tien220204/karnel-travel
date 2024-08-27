using Duende.IdentityServer.Models;
using Duende.IdentityServer;

namespace KarnelTravel.API;

public static class IdentitySeverConfigs
{
	public static IEnumerable<ApiResource> GetApiResources(IConfiguration configuration)
	{
		var apiResources = new List<ApiResource>
		{
			new ApiResource(IdentityServerConstants.StandardScopes.OfflineAccess),
			new ApiResource(IdentityServerConstants.StandardScopes.OpenId),
		};

		return apiResources;
	}

	public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
	{
		new IdentityResources.OpenId(),
		new IdentityResources.Profile(),
		new IdentityResources.Email(),
	};

	public static IEnumerable<ApiScope> ApiScopes =>
		new ApiScope[]
		{
            // Catalog API specific scopes
            // new ApiScope(name: PermissionScope.CatalogProductsRead,  displayName: "Read all catalog data."),
            // new ApiScope(name: PermissionScope.CatalogProductsReadWrite,  displayName:"Read and write all catalog data.")
        };

	public static IEnumerable<Client> GetClients(IConfiguration configuration)
	{
		var clients = new List<Client>
		{
			new Client
			{
				ClientId = IdentityClients.MOBILE,
				AllowedGrantTypes =
				{
					GrantType.ResourceOwnerPassword,
					"otp"
				},
				AllowOfflineAccess = true,

				AccessTokenLifetime = 86400,
				RefreshTokenUsage = TokenUsage.OneTimeOnly,
				RefreshTokenExpiration = TokenExpiration.Sliding,

				ClientSecrets =
				{
					new Secret(IdentityClients.CLIENT_SECRET.GetValueOrDefault(IdentityClients.MOBILE).Sha256())
				},
				AllowedScopes =
				{
					IdentityServerConstants.StandardScopes.OpenId,
					IdentityServerConstants.StandardScopes.Profile,
					IdentityServerConstants.StandardScopes.OfflineAccess
				}
			},

			new Client
			{
				ClientId = IdentityClients.PORTAL,
				AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
				AllowOfflineAccess = false,
				AccessTokenLifetime = 86400,

				ClientSecrets =
				{
					new Secret(IdentityClients.CLIENT_SECRET.GetValueOrDefault(IdentityClients.PORTAL).Sha256())
				},
				AllowedScopes =
				{
					IdentityServerConstants.StandardScopes.OpenId,
					IdentityServerConstants.StandardScopes.Profile
				}
			},
		};

		return clients;
	}
}
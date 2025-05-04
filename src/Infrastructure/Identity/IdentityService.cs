using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Domain.Entities.Features.Users;
using KarnelTravel.Share.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Share.Common.Extensions;

namespace KarnelTravel.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
	//private readonly ApplicationUser _userManager;
	//private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
	private readonly IKeycloakService _keycloakService;
	private readonly IApplicationDbContext _context;
	private readonly IAuthorizationService _authorizationService;

	public IdentityService(
		IKeycloakService keycloakService,
		IApplicationDbContext context,
		IAuthorizationService authorizationService)
	{
		_keycloakService = keycloakService;
		_context = context;
		_authorizationService = authorizationService;
	}

	public async Task<string?> GetUserNameAsync(string userId)
	{
		var user = await _context.ApplicationUsers.FindAsync(Guid.Parse(userId));

		return user?.FullName;
	}



	public async Task<ApplicationUser?> GetUserByKeycloakIdAsync(string keycloakId)
	{
		return await _context.ApplicationUsers
			.FirstOrDefaultAsync(u => u.KeycloakId == keycloakId);
	}

	//public async Task<AppActionResultData<string>> CreateUserAsync(string fullName, string password)
	//{
	//	var result = new AppActionResultData<string>();
	//	var user = new ApplicationUser
	//	{
	//		FullName = fullName,

	//	};

	//	var newUser = await _context.ApplicationUsers.(user, password);

	//	return result.BuildResult(user.Id.ToString());
	//}

	public async Task<AppActionResultData<string>> CreateUserAsync(string userName, string password)
	{
		var result = new AppActionResultData<string>();

		// Tạo user trên Keycloak (bạn cần implement gọi API ở KeycloakService)
		var keycloakUserId = await _keycloakService.CreateUserAsync(userName, password);

		if (keycloakUserId == null)
		{
			return result.BuildError("Tạo user trên Keycloak thất bại");
		}

		// Lưu thông tin user vào DB
		var appUser = new ApplicationUser
		{

			KeycloakId = keycloakUserId,
			FullName = userName,

		};

		await _context.ApplicationUsers.AddAsync(appUser);
		await _context.SaveChangesAsync(CancellationToken.None);

		return result.BuildResult(appUser.Id.ToString());
	}

	//public async Task<bool> IsInRoleAsync(string userId, string role)
	//{
	//	var user = await _userManager.FindByIdAsync(userId);

	//	return user != null && await _userManager.IsInRoleAsync(user, role);
	//}

	public async Task<bool> IsInRoleAsync(string userKeycloakId, string role)
	{
		if (userKeycloakId.IsNullOrEmpty())
		{
			return false;
		}

		var userRoleList = await _keycloakService.GetUserRolesAsync(userKeycloakId);

		return userRoleList.Contains(role);
	}


	//public async Task<bool> AuthorizeAsync(string userId, string policyName)
	//{
	//	var user = await _context.ApplicationUsers.FindAsync(userId);

	//	if (user == null)
	//	{
	//		return false;
	//	}

	//	var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

	//	var result = await _authorizationService.AuthorizeAsync(principal, policyName);

	//	return result.Succeeded;
	//}

	public async Task<AppActionResultData<string>> DeleteUserAsync(string userId)
	{
		var result = new AppActionResultData<string>();

		var user = await _context.ApplicationUsers.FindAsync(userId);
		if (user is null)
		{
			return result.BuildError("Không tìm thấy user");
		}

		return result.BuildResult(user.Id.ToString());
	}

	public async Task<AppActionResultData<string>> DeleteUserAsync(ApplicationUser user)
	{
		var result = new AppActionResultData<string>();

		_context.ApplicationUsers.Remove(user);
		await _context.SaveChangesAsync(CancellationToken.None);

		return result.BuildResult(user.Id.ToString());
	}
}

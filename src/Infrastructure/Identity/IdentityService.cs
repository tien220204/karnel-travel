using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Domain.Entities.Features.Users;
using KarnelTravel.Share.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KarnelTravel.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
	private readonly IAuthorizationService _authorizationService;

	public IdentityService(
		UserManager<ApplicationUser> userManager,
		IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
		IAuthorizationService authorizationService)
	{
		_userManager = userManager;
		_userClaimsPrincipalFactory = userClaimsPrincipalFactory;
		_authorizationService = authorizationService;
	}

	public async Task<string?> GetUserNameAsync(string userId)
	{
		var user = await _userManager.FindByIdAsync(userId);

		return user?.UserName;
	}

	public async Task<AppActionResultData<string>> CreateUserAsync(string userName, string password)
	{
		var result = new AppActionResultData<string>();
		var user = new ApplicationUser
		{
			UserName = userName,
			Email = userName,
		};

		var newUser = await _userManager.CreateAsync(user, password);

		return result.BuildResult(user.Id.ToString());
	}

	public async Task<bool> IsInRoleAsync(string userId, string role)
	{
		var user = await _userManager.FindByIdAsync(userId);

		return user != null && await _userManager.IsInRoleAsync(user, role);
	}

	public async Task<bool> AuthorizeAsync(string userId, string policyName)
	{
		var user = await _userManager.FindByIdAsync(userId);

		if (user == null)
		{
			return false;
		}

		var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

		var result = await _authorizationService.AuthorizeAsync(principal, policyName);

		return result.Succeeded;
	}

	public async Task<AppActionResultData<string>> DeleteUserAsync(string userId)
	{
		var result = new AppActionResultData<string>();

		var user = await _userManager.FindByIdAsync(userId);
		if (user is null)
		{
			return result.BuildError("Không tìm thấy user");
		}

		return result.BuildResult(user.Id.ToString());
	}

	public async Task<AppActionResultData<string>> DeleteUserAsync(ApplicationUser user)
	{
		var result = new AppActionResultData<string>();

		var delete = await _userManager.DeleteAsync(user);

		return result.BuildResult(user.Id.ToString());
	}
}

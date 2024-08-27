using Ardalis.GuardClauses;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using KarnelTravel.API.Extensions;
using KarnelTravel.Domain.Entities.Features.Users;
using Microsoft.AspNetCore.Identity;

namespace KarnelTravel.API.Custom;

public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly SignInManager<ApplicationUser> _signInManager;
	private readonly UserManager<ApplicationUser> _userManager;

	public ResourceOwnerPasswordValidator(
		IHttpContextAccessor httpContextAccessor,
		UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager)
	{
		_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
		_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
		_signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
	}

	public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
	{
		string ipAddress = _httpContextAccessor.HttpContext?.GetClientIpAddress() ?? string.Empty;
		string password = context.Password;
		string loginType = context.Request.Raw.Get("login_type") ?? string.Empty;

		ApplicationUser? user = null;

		switch (loginType)
		{
			case nameof(ApplicationUser.Email):
				user = await _userManager.FindByEmailAsync(context.UserName);
				break;

			case nameof(ApplicationUser.UserName):
				user = await _userManager.FindByNameAsync(context.UserName);
				break;
		}

		if (user == null)
		{
			throw new NotFoundException(nameof(ApplicationUser), context.UserName);
		}

		var signInResult = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
		if (signInResult.Succeeded)
		{
			var claims = (await _userManager.GetClaimsAsync(user)).ToList();
			context.Result = new GrantValidationResult(subject: user.Id, authenticationMethod: "password", claims: claims);
			return;
		}

		context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "The username and password do not match");
	}
}

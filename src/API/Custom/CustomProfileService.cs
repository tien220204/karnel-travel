//using Duende.IdentityServer.AspNetIdentity;
//using Duende.IdentityServer.Models;
//using KarnelTravel.Domain.Entities.Features.Users;
//using Microsoft.AspNetCore.Identity;
//using System.Security.Claims;

//namespace KarnelTravel.API.Custom;

//public class CustomProfileService : ProfileService<ApplicationUser>
//{
//	public CustomProfileService(
//		UserManager<ApplicationUser> userManager,
//		IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory) : base(userManager, claimsFactory)
//	{
//	}

//	protected override async Task GetProfileDataAsync(ProfileDataRequestContext context, ApplicationUser user)
//	{
//		var principal = await GetUserClaimsAsync(user);

//		var claims = new List<Claim>
//		{
//			new Claim(ClaimTypes.Name, user.UserName),
//			new Claim(ClaimTypes.Email, user.Email),
//			new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
//		};

//		context.IssuedClaims.AddRange(claims);
//		context.AddRequestedClaims(principal.Claims);
//	}
//}
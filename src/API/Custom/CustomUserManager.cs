using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Domain.Entities.Features.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace KarnelTravel.API.Custom;

public class CustomUserManager<TUser> : UserManager<TUser> where TUser : ApplicationUser
{
	private readonly IdentityOptions _option;
	private readonly IUser _currentUser;

	public CustomUserManager(
		IUserStore<TUser> store,
		IOptions<IdentityOptions> optionsAccessor,
		IPasswordHasher<TUser> passwordHasher,
		IEnumerable<IUserValidator<TUser>> userValidators,
		IEnumerable<IPasswordValidator<TUser>> passwordValidators,
		ILookupNormalizer keyNormalizer,
		IdentityErrorDescriber errors,
		IServiceProvider services,
		IUser currentUser,
		ILogger<UserManager<TUser>> logger
	)
		: base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
	{
		_option = optionsAccessor?.Value ?? new IdentityOptions();
		_currentUser = currentUser;
	}

	public override Task<IdentityResult> UpdateAsync(TUser user)
	{
		user.LastModifiedBy = _currentUser.Id ?? string.Empty;
		user.LastModified = DateTime.UtcNow;

		return base.UpdateAsync(user);
	}

	public override Task<IdentityResult> CreateAsync(TUser user)
	{
		user.CreatedBy = _currentUser.Id ?? string.Empty;
		user.Created = DateTime.UtcNow;

		return base.CreateAsync(user);
	}

	public override Task<IdentityResult> CreateAsync(TUser user, string password)
	{
		user.CreatedBy = _currentUser.Id ?? string.Empty;
		user.Created = DateTime.UtcNow;

		return base.CreateAsync(user, password);
	}
}

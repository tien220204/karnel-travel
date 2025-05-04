using KarnelTravel.Application.Common;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Share.Localization;
using MediatR;

namespace KarnelTravel.Application.Features.ApplicationUsers.Commands;

public class CreateApplicationUserCommand : IRequest<AppActionResultData<string>>
{
	public string Username { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }

}

public class CreateApplicationUserCommandHandler : BaseHandler, IRequestHandler<CreateApplicationUserCommand, AppActionResultData<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IIdentityService _identityService;

	public CreateApplicationUserCommandHandler(IApplicationDbContext context, IIdentityService identityService)
	{
		_context = context;
		_identityService = identityService;
	}

	public async Task<AppActionResultData<string>> Handle(CreateApplicationUserCommand request, CancellationToken cancellationToken)
	{
		var result = new AppActionResultData<string>();

		var isExistingInfo = _context.ApplicationUsers.Any(x =>  x.Email == request.Email);

		if (isExistingInfo)
		{
			return BuildMultilingualError(result, Resources.ERR_MSG_DATA_EXISTED);
		}

		var newUser = await _identityService.CreateUserAsync(request.Username, request.Password);

		await _context.SaveChangesAsync(cancellationToken);

		return BuildMultilingualResult(result, newUser.Data, Resources.INF_MSG_SAVE_SUCCESSFULLY);
	}
}

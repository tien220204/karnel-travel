using FluentValidation;
using KarnelTravel.Application.Features.ApplicationUsers.Commands;

namespace KarnelTravel.Application.Features.ApplicationUsers.Models.Validators;
public class CreateApplicationUserCommandValidator : AbstractValidator<CreateApplicationUserCommand>
{
	public CreateApplicationUserCommandValidator()
	{
		RuleFor(x => x.Username)
			.Matches("^[a-zA-Z0-9]+$").WithMessage("username must not contain spaces or special characters.")
			.NotEmpty().WithMessage("Farmer name is required.")
			.NotNull().WithMessage("Farmer name is required.");

		RuleFor(x => x.Password)
			.Matches("^[a-zA-Z0-9]+$").WithMessage("password must not contain spaces or special characters.")
			.NotEmpty().WithMessage("Farmer name is required.")
			.NotNull().WithMessage("Farmer name is required.");
	}
}

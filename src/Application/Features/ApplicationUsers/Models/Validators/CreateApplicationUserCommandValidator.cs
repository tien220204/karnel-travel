using FluentValidation;
using KarnelTravel.Application.Features.ApplicationUsers.Commands;

namespace KarnelTravel.Application.Features.ApplicationUsers.Models.Validators;
public class CreateApplicationUserCommandValidator : AbstractValidator<CreateApplicationUserCommand>
{
	public CreateApplicationUserCommandValidator()
	{
		RuleFor(x => x.Username)
			.Matches("^[a-zA-Z0-9]+$").WithMessage("username must not contain spaces or special characters.")
			.NotEmpty().WithMessage("Username is required.")
			.NotNull().WithMessage("User name is required.");

		RuleFor(x => x.Password)
			.Matches("^[a-zA-Z0-9]+$").WithMessage("password must not contain spaces or special characters.")
			.NotEmpty().WithMessage("Password is required.")
			.NotNull().WithMessage("Password is required.");

		RuleFor(x => x.Email)
			.EmailAddress().WithMessage("Email is not valid.")
			.NotEmpty().WithMessage("Email is required.")
			.NotNull().WithMessage("Email is required.");
	}
}

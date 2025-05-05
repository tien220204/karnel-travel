using FluentValidation;
using KarnelTravel.Application.Feature.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.Hotels.Models.Validators.Hotels;
public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
	public CreateHotelCommandValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Name is required.")
			.NotNull().WithMessage("Name is required.")
			.MinimumLength(20).WithMessage("Name must be at least 20 characters long.")
			.Matches("^[a-zA-Z0-9 ]+$").WithMessage("Name must not contain special characters.");
		
	}
}

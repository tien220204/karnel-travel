using FluentValidation;
using KarnelTravel.Application.Features.Flights.Commands.Airports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.Flights.Models.Validators;


public class UpdateAirportCommandValidator : AbstractValidator<UpdateAirportCommand>
{
	public UpdateAirportCommandValidator()
	{
		RuleFor(x => x.AirportCode)
			.NotEmpty()
			.WithMessage("Airport code is required.")
			.Length(3, 10)
			.WithMessage("Airport code must be between 3 and 10 characters.");

		RuleFor(x => x.Name)
			.NotEmpty()
			.WithMessage("Airport name is required.")
			.Length(3, 100)
			.WithMessage("Airport name must be between 3 and 100 characters.");

		RuleFor(x => x.ProvinceCode)
			.NotEmpty()
			.WithMessage("Province code is required.");

		RuleFor(x => x.CountryCode)
			.NotEmpty()
			.WithMessage("Country code is required.");

		RuleFor(x => x.Timezone)
			.NotEmpty().WithMessage("Country code is required.")
			.Must(BeValidUtcOffset).WithMessage("Timezone must be in UTC type (-12 to +14)");

	}

	private bool BeValidUtcOffset(string offset)
	{
		if (int.TryParse(offset.Replace("+", ""), out int value))
		{
			return value >= -12 && value <= 14;
		}
		return false;
	}
}

using FluentValidation;
using KarnelTravel.Application.Features.Hotels.Commands.HotelRating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.Hotels.Models.Validators.HotelRatings;
public class CreateHotelRatingCommandValidator : AbstractValidator<CreateHotelRatingCommand>
{
	public CreateHotelRatingCommandValidator()
	{
		RuleFor(x => x.StarRate)
			.InclusiveBetween(0,5).WithMessage("star rating must not contain spaces or special characters.")
			.Must(x => x % 1 == 0).WithMessage("star rating must be an integer."); ;
	}
}

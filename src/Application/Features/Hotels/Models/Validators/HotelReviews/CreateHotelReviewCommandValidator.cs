using FluentValidation;
using KarnelTravel.Application.Features.Hotels.Commands.HotelReview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Application.Features.Hotels.Models.Validators.HotelReviews;
public class CreateHotelReviewCommandValidator : AbstractValidator<CreateHotelReviewCommand>
{
	public CreateHotelReviewCommandValidator()
	{
		RuleFor(x => x.Review)
			.NotEmpty().WithMessage("Review is required.")
			.NotNull().WithMessage("Review is required.")
			.MinimumLength(225).WithMessage("Review must be at least 225 characters long.")
			.Matches("^[a-zA-Z0-9 ]+$").WithMessage("Review must not contain special characters.");

		
	}
}

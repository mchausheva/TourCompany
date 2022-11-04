using FluentValidation;
using TourCompany.Models.Requests;

namespace TourCompany.Validations
{
    public class ReservationRequestValidator : AbstractValidator<ReservationRequest>
    {
        public ReservationRequestValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();

            RuleFor(x => x.ReservationDate).GreaterThan(DateTime.Now)
                                           .LessThan(DateTime.MaxValue);

            RuleFor(x => x.CityId).NotEmpty()
                                  .GreaterThan(99);

            RuleFor(x => x.Days).GreaterThanOrEqualTo(1).WithMessage("Days must be more than 0.")
                                .LessThanOrEqualTo(14).WithMessage("The Reservation can'ot be more than 14 days.");

            RuleFor(x => x.NumberOfPeople).GreaterThanOrEqualTo(1)
                                          .WithMessage("The count of people must be more than 0");

            When(x => !string.IsNullOrEmpty(x.PromoCode), () =>
            {
                RuleFor(x => x.PromoCode)
                       .MinimumLength(10).MaximumLength(11)
                       .Must(x => x.EndsWith('%')).WithMessage("Promo CODE is invalid");
            });
        }
    }
}

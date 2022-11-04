using FluentValidation;
using TourCompany.Models.Requests;

namespace TourCompany.Validations
{
    public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
    {
        public CustomerRequestValidator()
        {
            RuleFor(x => x.CustomerName).NotEmpty();

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email address is required.")
                                 .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.Telephone).Length(10).WithMessage("The number must be 10 digits.")
                                     .Must(x => x.StartsWith("08")).WithMessage("The number starts with 08********.");
        }
    }
}

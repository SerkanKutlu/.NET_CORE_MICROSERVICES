using CustomerService.Domain.ValueObjects;
using FluentValidation;

namespace CustomerService.Application.Validations;

public class AddressValidation:AbstractValidator<Address>
{
    public AddressValidation()
    {
        RuleFor(a => a.City).NotEmpty().NotNull().MaximumLength(20).WithMessage("City can not be longer than 20 characters");
        RuleFor(a => a.Country).NotEmpty().NotNull().MaximumLength(20).WithMessage("Country can not be longer than 20 characters");
        RuleFor(a => a.AddressLine).NotEmpty().NotNull();
        RuleFor(a => a.CityCode).NotNull().Must(c => c > 0).WithMessage("CityCode has to be greater than 0");
    }
}
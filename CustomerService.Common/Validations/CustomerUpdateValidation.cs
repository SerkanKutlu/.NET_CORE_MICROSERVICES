using CustomerService.Common.DTO;
using FluentValidation;

namespace CustomerService.Common.Validations;

public class CustomerUpdateValidation:AbstractValidator<CustomerForUpdateDto>
{
    public CustomerUpdateValidation()
    {
        RuleFor(c => c.Name).NotEmpty().NotNull().MaximumLength(20).WithMessage("Name can not be longer than 20 characters");
        RuleFor(c => c.Email).NotEmpty().EmailAddress().WithMessage("Enter a valid email address");
        RuleFor(c => c.Address).SetValidator(new AddressValidation());
    }
}
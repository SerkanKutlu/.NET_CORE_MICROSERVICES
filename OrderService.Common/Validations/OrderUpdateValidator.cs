using FluentValidation;
using OrderService.Common.DTO;

namespace OrderService.Common.Validations;

public class OrderUpdateValidator:AbstractValidator<OrderForUpdateDto>
{

    public OrderUpdateValidator()
    {
        RuleFor(o => o.Id).NotEmpty().NotNull();
        RuleFor(o => o.Status).NotEmpty().NotNull();
        RuleForEach(o => o.ProductIds).NotEmpty().NotNull();
        RuleFor(o => o.Address).SetValidator(new AddressValidator());

    }
    
}
using FluentValidation;
using OrderService.Common.DTO;

namespace OrderService.Common.Validations;

public class OrderCreateValidator:AbstractValidator<OrderForCreationDto>
{

    public OrderCreateValidator()
    {
        RuleFor(o=>o.CustomerId).NotEmpty().NotNull();
        RuleFor(o => o.Status).NotEmpty().NotNull();
        RuleForEach(o => o.ProductIds).NotEmpty().NotNull();
    }
    
}
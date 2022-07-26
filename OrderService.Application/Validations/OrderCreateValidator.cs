using FluentValidation;
using OrderService.Application.DTO;

namespace OrderService.Application.Validations;

public class OrderCreateValidator:AbstractValidator<OrderForCreationDto>
{

    public OrderCreateValidator()
    {
        RuleFor(o=>o.CustomerId).NotEmpty().NotNull();
        RuleFor(o => o.Status).NotEmpty().NotNull();
        RuleForEach(o => o.ProductIds).NotEmpty().NotNull();
    }
    
}
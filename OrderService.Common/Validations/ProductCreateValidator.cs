using FluentValidation;
using OrderService.Common.DTO;

namespace OrderService.Common.Validations;

public class ProductCreateValidator:AbstractValidator<ProductForCreationDto>
{
    public ProductCreateValidator()
    {
        RuleFor(p => p.Name).NotEmpty().NotNull();
        RuleFor(p => p.Price).NotEmpty().NotNull();
        RuleFor(p => p.ImageUrl).NotEmpty().NotNull();
    }
}
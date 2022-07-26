using OrderService.Application.DTO;
using FluentValidation;
namespace OrderService.Application.Validations;

public class ProductCreateValidator:AbstractValidator<ProductForCreationDto>
{
    public ProductCreateValidator()
    {
        RuleFor(p => p.Name).NotEmpty().NotNull();
        RuleFor(p => p.Price).NotEmpty().NotNull();
        RuleFor(p => p.ImageUrl).NotEmpty().NotNull();
    }
}
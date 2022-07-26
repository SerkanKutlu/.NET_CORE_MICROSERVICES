using OrderService.Application.DTO;
using FluentValidation;
namespace OrderService.Application.Validations;

public class ProductUpdateValidator : AbstractValidator<ProductForUpdateDto>
{

    public ProductUpdateValidator()
    {
        RuleFor(p => p.Id).NotEmpty().NotNull();
        RuleFor(p => p.Name).NotEmpty().NotNull();
        RuleFor(p => p.Price).NotEmpty().NotNull();
        RuleFor(p => p.ImageUrl).NotEmpty().NotNull();
    }
    
}
using FluentValidation;

namespace Project.Application.Features.Products.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre no puede exceder 150 caracteres.");

        RuleFor(p => p.SKU)
            .NotEmpty().WithMessage("El SKU es obligatorio.")
            .MinimumLength(3).WithMessage("El SKU debe tener al menos 3 caracteres.");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a cero.");
    }
}
using FluentValidation;
using HxFood.Api.Models.Requests;
using HxFood.Api.Models.Requests.Product;

namespace HxFood.Api.Services.Validators
{
    public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
    {
        public ProductUpdateRequestValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price is required.");
            RuleFor(p => p.Currency).NotEmpty().WithMessage("Currency is required.");
            RuleFor(p => p.CategoryId).NotEmpty().WithMessage("CategoryId is required.");
        }
    }
}
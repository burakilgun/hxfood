using FluentValidation;
using HxFood.Api.Models.Requests.Category;

namespace HxFood.Api.Services.Validators
{
    public class CategoryUpdateRequestValidator : AbstractValidator<CategoryUpdateRequest>
    {
        public CategoryUpdateRequestValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required.");
        }
    }
}
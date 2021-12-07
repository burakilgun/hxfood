using FluentValidation;
using HxFood.Api.Models.Requests.Category;

namespace HxFood.Api.Services.Validators
{
    public class CategoryAddRequestValidator : AbstractValidator<CategoryAddRequest>
    {
        public CategoryAddRequestValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required.");
        }
    }
}
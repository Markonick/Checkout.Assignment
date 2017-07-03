using Checkout.ShoppingService.Models;
using FluentValidation;

namespace Checkout.ShoppingService.Validators
{
    public class NameValidator : AbstractValidator<DrinkModel>
    {
        public NameValidator()
        {
            RuleFor(req => req.Name).NotEmpty().NotNull().WithMessage("Cannot be empty or null");
        }
    }
}

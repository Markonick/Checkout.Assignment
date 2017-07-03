using System.Configuration;
using Checkout.ShoppingService.Models;
using FluentValidation;

namespace Checkout.ShoppingService.Validators
{
    public class QuantityValidator : AbstractValidator<DrinkModel>
    {
        public QuantityValidator()
        {
            var maxNumberOfDrinks = int.Parse(ConfigurationManager.AppSettings["MaxNumberOfDrinks"]);

            RuleFor(req => int.Parse(req.Quantity)).GreaterThan(0).WithMessage("Drink quantity must be greater than 0");
            RuleFor(req => int.Parse(req.Quantity)).LessThan(maxNumberOfDrinks).WithMessage(x => $"Cannot order more than {maxNumberOfDrinks}");
        }
    }
}
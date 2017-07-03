using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkout.ShoppingService.Models;
using FluentValidation;

namespace Checkout.ShoppingService
{
    public class FormatValidator : AbstractValidator<DrinkModel>
    {
        public FormatValidator()
        {
            RuleFor(req => req.Quantity).Must(IsNumerical).WithMessage("Invalid quantity format. Must be a numerical value");
        }

        public bool IsNumerical(string input)
        {
            int number;
            return int.TryParse(input, out number);
        }
    }
}

using System.Collections.Generic;
using Checkout.ShoppingService.Models;
using FluentValidation;

namespace Checkout.ShoppingService.Validators
{
    public interface IValidateRequest
    {
        string GetResult(DrinkModel input);
    }
}
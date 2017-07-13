using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Checkout.ShoppingService.Models;
using FluentValidation;

namespace Checkout.ShoppingService.Validators
{
    public class ValidateRequest : IValidateRequest
    {
        private readonly IList<AbstractValidator<DrinkModel>> _validators;

        public ValidateRequest(IList<AbstractValidator<DrinkModel>> validators)
        {
            _validators = validators;
        }

        public string GetResult(DrinkModel input)
        {
            try
            {
                foreach (var validator in _validators)
                {
                    var nameValidationResult = validator.Validate(input);

                    if (!nameValidationResult.IsValid)
                    {
                        return nameValidationResult.Errors.First().ErrorMessage;
                    }
                }

                return "Passed request validations!";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
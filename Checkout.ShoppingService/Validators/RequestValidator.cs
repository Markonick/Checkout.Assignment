using System;
using System.Linq;
using System.Xml;
using Checkout.ShoppingService.Models;
using FluentValidation;

namespace Checkout.ShoppingService.Validators
{
    public class RequestValidator : IRequestValidator
    {
        private readonly AbstractValidator<DrinkModel> _formatValidator;
        private readonly AbstractValidator<DrinkModel> _nameValidator;
        private readonly AbstractValidator<DrinkModel> _quantityValidator;

        public RequestValidator(AbstractValidator<DrinkModel> formatValidator, AbstractValidator<DrinkModel> nameValidator, AbstractValidator<DrinkModel> quantityValidator)
        {
            _formatValidator = formatValidator;
            _nameValidator = nameValidator;
            _quantityValidator = quantityValidator;
        }

        public string GetResult(DrinkModel input, bool validateBoth)
        {
            try
            {
                var nameValidationResult = _nameValidator.Validate(input);

                if (!nameValidationResult.IsValid)
                {
                    return nameValidationResult.Errors.FirstOrDefault().ErrorMessage;
                }
                if (!validateBoth)
                {
                    return "Passed request validations!";
                }
                var formatValidationResult = _formatValidator.Validate(input);

                if (!formatValidationResult.IsValid)
                {
                    return formatValidationResult.Errors.FirstOrDefault().ErrorMessage;
                }
                
                var quantityValidationResult = _quantityValidator.Validate(input);

                if (!quantityValidationResult.IsValid)
                {
                    return quantityValidationResult.Errors.FirstOrDefault().ErrorMessage;
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
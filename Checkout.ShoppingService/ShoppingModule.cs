using System;
using System.Collections.Generic;
using Checkout.ShoppingService.Models;
using Checkout.ShoppingService.Repositories;
using Checkout.ShoppingService.Validators;
using FluentValidation;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Security;

namespace Checkout.ShoppingService
{
    public class ShoppingModule : NancyModule
    {
        private readonly IShoppingRepository _repository;

        public ShoppingModule(IShoppingRepository repository)
        {
            _repository = repository;

            Get["/shopping"] = args => GetDrinks();

            Get["/shopping/{name}"] = args => GetDrink(args);

            Post["/shopping/{name}/{quantity}"] = args => AddDrink(args);

            Put["/shopping/{name}/{quantity}"] = args => UpdateDrink(args);

            Delete["/shopping/{name}"] = args => DeleteDrink(args);
        }

        private Negotiator GetDrinks()
        {
            try
            {
                //this.RequiresAuthentication();
                //Service
                var result = _repository.GetDrinksList();

                if (result == null)
                {
                    var errorMessage = new ErrorMessage { Message = "NotFound" };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.NotFound);
                }

                return Negotiate.WithModel(result).WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                var errorMessage = new ErrorMessage { Message = "InternalServerError" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private Negotiator GetDrink(dynamic args)
        {
            try
            {
                //this.RequiresAuthentication();
                var drinkModel = new DrinkModel { Name = args.name, Quantity = args.quantity };

                //Validation
                var validateRequest = new ValidateRequest(new List<AbstractValidator<DrinkModel>> {new NameValidator()});
            
                var validationResult = validateRequest.GetResult(drinkModel);

                if (validationResult != "Passed request validations!")
                {
                    var errorMessage = new ErrorMessage { Message = validationResult };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.BadRequest);
                }

                //Service
                Drink result = _repository.GetDrink(args.name);

                if (result == null)
                {
                    var errorMessage = new ErrorMessage { Message = "NotFound" };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.NotFound);
                }

                return Negotiate.WithModel(result).WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                var errorMessage = new ErrorMessage { Message = "InternalServerError" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private Negotiator AddDrink(dynamic args)
        {
            try
            {
                //this.RequiresAuthentication();
                var drinkModel = new DrinkModel { Name = args.name, Quantity = args.quantity };

                //Validation
                var validateRequest = new ValidateRequest(new List<AbstractValidator<DrinkModel>> { new NameValidator(), new FormatValidator(), new QuantityValidator()});

                var validationResult = validateRequest.GetResult(drinkModel);

                if (validationResult != "Passed request validations!")
                {
                    var errorMessage = new ErrorMessage { Message = validationResult };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.BadRequest);
                }

                //Service
                var drink = new Drink {Name = args.name, Quantity = args.quantity};
                var result = _repository.AddDrink(drink);

                if (result == false)
                {
                    var errorMessage = new ErrorMessage { Message = "Error adding item" };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.BadRequest);
                }

                return Negotiate.WithModel(drink).WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                var errorMessage = new ErrorMessage { Message = "InternalServerError" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private Negotiator UpdateDrink(dynamic args)
        {
            try
            {
                //this.RequiresAuthentication();
                var drinkModel = new DrinkModel { Name = args.name, Quantity = args.quantity };

                //Validation
                var validateRequest = new ValidateRequest(new List<AbstractValidator<DrinkModel>> { new NameValidator(), new FormatValidator(), new QuantityValidator() });

                var validationResult = validateRequest.GetResult(drinkModel);

                if (validationResult != "Passed request validations!")
                {
                    var errorMessage = new ErrorMessage { Message = validationResult };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.BadRequest);
                }

                //Service
                var drink = new Drink { Name = args.name, Quantity = args.quantity };
                var result = _repository.UpdateDrink(drink);
                
                if (result == false)
                {
                    var errorMessage = new ErrorMessage { Message = "NotFound" };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.NotFound);
                }

                var message = new OkResponse { Message = "OK" };
                return Negotiate.WithModel(message).WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                var errorMessage = new ErrorMessage { Message = "InternalServerError" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private Negotiator DeleteDrink(dynamic args)
        {
            try
            {
                //this.RequiresAuthentication();
                var drinkModel = new DrinkModel { Name = args.name, Quantity = args.quantity };

                //Validation
                var validateRequest = new ValidateRequest(new List<AbstractValidator<DrinkModel>> { new NameValidator() });

                var validationResult = validateRequest.GetResult(drinkModel);

                if (validationResult != "Passed request validations!")
                {
                    var errorMessage = new ErrorMessage { Message = validationResult };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.BadRequest);
                }

                //Service
                var result = _repository.DeleteDrink(args.name);

                if (result == false)
                {
                    var errorMessage = new ErrorMessage { Message = "NotFound" };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.NotFound);
                }

                var message = new OkResponse {Message = "OK"};
                return Negotiate.WithModel(message).WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                var errorMessage = new ErrorMessage {Message = "InternalServerError"};
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }
    }
}

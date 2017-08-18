using System;
using System.Collections.Generic;
using System.Net;
using Checkout.ApiServices.Shopping.RequestModels;
using NUnit.Framework;
using Nancy.Testing;
using Checkout.ShoppingService.Models;
using Checkout.ShoppingService.Repositories;
using Checkout.ShoppingService.Validators;
using Moq;
using Newtonsoft.Json;
using DrinkModel = Checkout.ShoppingService.Models.DrinkModel;

namespace Checkout.ShoppingService.Tests
{
    public class ShoppingModuleTests
    {
        private Mock<IShoppingRepository> _repository;
        private ShoppingModule _module;
        private Browser _browser;
        private Mock<IValidateRequest> _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new Mock<IValidateRequest>();
            _repository = new Mock<IShoppingRepository>();
            _module = new ShoppingModule(_repository.Object);
            _browser = new Browser(with => with.Module(_module));
        }

        [TearDown]
        public void Teardown()
        {
            //TODO:
        }

        [TestCase("Fanta", "100", "10", "0")]
        [TestCase("San Pellegrino", "10", "10", "0")]
        public void GetDrinks_empty_should_return_notFound(string name, string quantity, string count, string offset)
        {
            var emptyList = new List<Drink>();

            var startTime = DateTime.UtcNow.AddHours(-1);
            _repository.Setup(repo => repo.GetDrinksList(count, offset, startTime, DateTime.UtcNow)).Returns(emptyList);

            var response = _browser.Get("/shopping", with => {
                with.Header("Accept", "application/json");
            });

            Assert.That(response.StatusCode.ToString(), Is.EqualTo(HttpStatusCode.NotFound.ToString()));
        }

        [TestCase("Fanta", "100")]
        [TestCase("San Pellegrino", "10")]
        public void CreateDrink(string name, string quantity)
        {
            var drink = new Drink {Name = name, Quantity = int.Parse(quantity)};
            var drinkRequest = new DrinkModel { Name = name, Quantity = quantity };
            
            _repository.Setup(repo => repo.AddDrink(drink)).Returns(true);
            _validator.Setup(validator => validator.GetResult(drinkRequest)).Returns("Passed request validations!");
            
            var response = _browser.Post("/shopping/name/quantity", with =>
            {
                with.JsonBody(drinkRequest);
            });
            
            Assert.That(response.StatusCode.ToString(), Is.EqualTo(HttpStatusCode.OK.ToString()));
        }

        [TestCase("Fanta", "100", "101")]
        [TestCase("San Pellegrino", "10", "20")]
        public void UpdateDrink(string name, string quantity, string updatedQuantity)
        {
            var drink = new Drink { Name = name, Quantity = int.Parse(updatedQuantity) };
            var drinkRequest = new DrinkModel { Name = name, Quantity = quantity };
            _repository.Setup(repo => repo.AddDrink(drink)).Returns(true);

            var addBody = drinkRequest;
            var addJson = JsonConvert.SerializeObject(addBody);

            var addResponse = _browser.Post("/name/quantity", with => {
                with.HttpRequest();
                with.Body(addJson, "application/json");
            });

            var updatedDrink = new Drink { Name = name, Quantity = int.Parse(quantity) };
            var updatedDrinkRequest = new DrinkModel { Name = name, Quantity = updatedQuantity };
            _repository.Setup(repo => repo.UpdateDrink(updatedDrink)).Returns(true);

            var updateBody = updatedDrinkRequest;
            var updateJson = JsonConvert.SerializeObject(updateBody);

            var updateResponse = _browser.Put("/shopping/name/quantity", with => {
                with.HttpRequest();
                with.Body(updateJson, "application/json");
            });
            
            Assert.That(updateResponse.StatusCode.ToString(), Is.EqualTo(HttpStatusCode.OK.ToString()));
        }
    }
}

﻿using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using Nancy.Testing;
using Checkout.ShoppingService.Models;
using Checkout.ShoppingService.Repositories;
using Checkout.ShoppingService.Validators;
using Moq;
using Newtonsoft.Json;

namespace Checkout.ShoppingService.Tests
{
    public class ShoppingModuleTests
    {
        private Mock<IShoppingRepository> _repository;
        private ShoppingModule _module;
        private Browser _browser;
        
        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IShoppingRepository>();
            _module = new ShoppingModule(_repository.Object);
            _browser = new Browser(with => with.Module(_module));
        }

        [TearDown]
        public void Teardown()
        {
            //TODO:
        }

        [TestCase("Fanta", "100")]
        [TestCase("San Pellegrino", "10")]
        public void GetDrinks_empty_should_return_notFound(string name, string quantity)
        {
            _repository.Setup(repo => repo.GetDrinksList()).Returns(new List<Drink>());

            var response = _browser.Get("/", with => {
                with.HttpRequest();
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

            var body = drinkRequest;
            var json = JsonConvert.SerializeObject(body);

            var response = _browser.Post("/name/quantity", with => {
                with.HttpRequest();
                with.Body(json, "application/json");
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

using System;
using System.Collections.Generic;
using System.Net;
using Checkout;
using Checkout.ApiServices.Shopping.RequestModels;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.ShoppingService
{
    [TestFixture(Category = "ShoppingApi")]
    public class ShoppingServiceTests : BaseServiceTests
    {
        private List<DrinkModel> _list;

        [SetUp]
        public void Setup()
        {
            CheckoutClient = new APIClient(new Uri("http://localhost:5000"));
        }

        [TearDown]
        public void Teardown()
        {
            var startTime = DateTime.UtcNow.AddHours(-1); // records for the past hour
            var drinkGetListRequest = new DrinkGetList
            {
                FromDate = startTime,
                ToDate = DateTime.UtcNow
            };
            
            _list = CheckoutClient.ShoppingService.GetShoppingList(drinkGetListRequest).Model.Data;

            foreach (var item in _list)
            {
                CheckoutClient.ShoppingService.DeleteDrink(item.Name);
            }
        }

        [Test]
        public void CreateDrink()
        {
            var drinkCreateModel = TestHelper.GetDrinkCreateModel("San Pellegrino", "10");
            var response = CheckoutClient.ShoppingService.AddDrink(drinkCreateModel);

            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            drinkCreateModel.ShouldBeEquivalentTo(response.Model);
        }

        [TestCase("San Pellegrino", "0")]
        [TestCase("Coca Cola", "-1")]
        public void CreateDrink_with_invalid_quantity_should_return_BadRequest(string name, string quantity)
        {
            var drinkCreateModel = TestHelper.GetDrinkCreateModel(name, quantity);
            var response = CheckoutClient.ShoppingService.AddDrink(drinkCreateModel);

            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestCase("San Pellegrino", "10", "20")]
        [TestCase("Fanta", "10", "11")]
        [TestCase("IPA", "1", "2")]
        public void CreateDrink_should_just_update_quantity_if_drink_already_exists(string name, string quantity, string updatedQuantity)
        {
            var drinkCreateModel = TestHelper.GetDrinkCreateModel(name, quantity);
            CheckoutClient.ShoppingService.AddDrink(drinkCreateModel);
            drinkCreateModel = TestHelper.GetDrinkCreateModel(name, updatedQuantity);
            CheckoutClient.ShoppingService.AddDrink(drinkCreateModel);

            var startTime = DateTime.UtcNow.AddHours(-1); // records for the past hour
            var drinkGetListRequest = new DrinkGetList
            {
                FromDate = startTime,
                ToDate = DateTime.UtcNow
            };

            var response = CheckoutClient.ShoppingService.GetShoppingList(drinkGetListRequest);

            response.Should().NotBeNull();
            response.Model.Count.Should().Be(1);
            response.Model.Data[0].Name.Should().Be(name);
            response.Model.Data[0].Quantity.Should().Be(updatedQuantity);
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestCase("San Pellegrino", "10", "15")]
        [TestCase("Fanta", "10", "1")]
        [TestCase("IPA", "1", "2")]
        public void UpdateDrink(string name, string quantity, string updatedQuantity)
        {
            var drinkCreateModel = TestHelper.GetDrinkCreateModel(name, quantity);
            var addResponse = CheckoutClient.ShoppingService.AddDrink(drinkCreateModel);

            var drinkUpdateModel = TestHelper.GetDrinkUpdateModel(name, quantity);
            var updateResponse = CheckoutClient.ShoppingService.UpdateDrink(drinkUpdateModel);

            updateResponse.Should().NotBeNull();
            updateResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            updateResponse.Model.Message.Should().BeEquivalentTo("Ok");
        }

        [TestCase("San Pellegrino", "10")]
        [TestCase("Coca Cola", "12")]
        [TestCase("IPA", "1")]
        public void UpdateDrink_when_drink_does_not_eist_should_return_NotFound(string name, string quantity)
        {
            var drinkCreateModel1 = TestHelper.GetDrinkCreateModel("Lagunitas", "2");
            CheckoutClient.ShoppingService.AddDrink(drinkCreateModel1);
            var drinkCreateModel2 = TestHelper.GetDrinkCreateModel("Fanta", "20");
            CheckoutClient.ShoppingService.AddDrink(drinkCreateModel2);
            var drinkCreateModel3 = TestHelper.GetDrinkCreateModel("Evian", "2");
            CheckoutClient.ShoppingService.AddDrink(drinkCreateModel3);
            var drinkCreateModel4 = TestHelper.GetDrinkCreateModel("Estrella", "2");
            CheckoutClient.ShoppingService.AddDrink(drinkCreateModel4);

            var drinkUpdateModel = TestHelper.GetDrinkUpdateModel(name, quantity);
            var updateResponse = CheckoutClient.ShoppingService.UpdateDrink(drinkUpdateModel);

            updateResponse.Should().NotBeNull();
            updateResponse.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public void DeleteDrink()
        {
            var drink = CheckoutClient.ShoppingService.AddDrink(TestHelper.GetDrinkCreateModel("Sprite", "1")).Model;
            var response = CheckoutClient.ShoppingService.DeleteDrink(drink.Name);

            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Message.Should().BeEquivalentTo("Ok");
        }

        [TestCase("San Pellegrino", "10")]
        [TestCase("Fanta", "10")]
        [TestCase("IPA", "1")]
        public void Deleting_from_empty_list_is_handled(string name, string quantity)
        {
            var drink = new DrinkModel {Name = name, Quantity = quantity};
            var response = CheckoutClient.ShoppingService.DeleteDrink(drink.Name);
            var startTime = DateTime.UtcNow.AddHours(-1); // records for the past hour
            var drinkGetListRequest = new DrinkGetList
            {
                FromDate = startTime,
                ToDate = DateTime.UtcNow
            };

            var list = CheckoutClient.ShoppingService.GetShoppingList(drinkGetListRequest);

            list.Model.Data.Should().BeEmpty();
            response.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestCase("San Pellegrino")]
        [TestCase("Coca Cola")]
        [TestCase("IPA")]
        public void Deleting_non_existing_drink_in_list_is_handled(string name)
        {
            var drinkCreateModel1 = TestHelper.GetDrinkCreateModel("Lagunitas", "2");
            CheckoutClient.ShoppingService.AddDrink(drinkCreateModel1);
            var drinkCreateModel2 = TestHelper.GetDrinkCreateModel("Fanta", "20");
            CheckoutClient.ShoppingService.AddDrink(drinkCreateModel2);
            var drinkCreateModel3 = TestHelper.GetDrinkCreateModel("Evian", "2");
            CheckoutClient.ShoppingService.AddDrink(drinkCreateModel3);
            var drinkCreateModel4 = TestHelper.GetDrinkCreateModel("Estrella", "2");
            CheckoutClient.ShoppingService.AddDrink(drinkCreateModel4);
            
            var response = CheckoutClient.ShoppingService.DeleteDrink(name);

            var startTime = DateTime.UtcNow.AddHours(-1); // records for the past hour
            var drinkGetListRequest = new DrinkGetList
            {
                FromDate = startTime,
                ToDate = DateTime.UtcNow
            };

            var list = CheckoutClient.ShoppingService.GetShoppingList(drinkGetListRequest);

            list.Should().NotBeNull();
            list.Model.Should().Be(4);
            response.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public void GetDrink()
        {
            var drinkCreateModel = TestHelper.GetDrinkCreateModel("Pepsi", "5");
            var drink = CheckoutClient.ShoppingService.AddDrink(drinkCreateModel).Model;

            var response = CheckoutClient.ShoppingService.GetDrink(drink.Name);

            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Name.Should().Be(drink.Name);
            drink.ShouldBeEquivalentTo(response.Model);
        }


        [TestCase("San Pellegrino")]
        [TestCase("Coca Cola")]
        [TestCase("IPA")]
        public void GetDrink_that_doesnt_exist_should_return_NotFound(string name)
        {
            var response = CheckoutClient.ShoppingService.GetDrink(name);

            response.Model.Should().BeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public void GetDrinkList()
        {
            var drinkCreateModel1 = TestHelper.GetDrinkCreateModel("Lagunitas", "2");
            var drink1 = CheckoutClient.ShoppingService.AddDrink(drinkCreateModel1);
            var drinkCreateModel2 = TestHelper.GetDrinkCreateModel("Fanta", "20");
            var drink2 = CheckoutClient.ShoppingService.AddDrink(drinkCreateModel2);
            var drinkCreateModel3 = TestHelper.GetDrinkCreateModel("Evian", "2");
            var drink3 = CheckoutClient.ShoppingService.AddDrink(drinkCreateModel3);
            var drinkCreateModel4 = TestHelper.GetDrinkCreateModel("Estrella", "2");
            var drink4 = CheckoutClient.ShoppingService.AddDrink(drinkCreateModel4);
            
            
            var startTime = DateTime.UtcNow.AddHours(-1); // records for the past hour
            var drinkGetListRequest = new DrinkGetList
            {
                FromDate = startTime,
                ToDate = DateTime.UtcNow
            };

            var response = CheckoutClient.ShoppingService.GetShoppingList(drinkGetListRequest);

            response.Model.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Count.Should().BeGreaterOrEqualTo(4);

            response.Model.Data[0].Name.Should().Be(drink1.Model.Name);
            response.Model.Data[1].Name.Should().Be(drink2.Model.Name);
            response.Model.Data[2].Name.Should().Be(drink3.Model.Name);
            response.Model.Data[3].Name.Should().Be(drink4.Model.Name);
            response.Model.Data[0].Quantity.Should().Be(drink1.Model.Quantity);
            response.Model.Data[1].Quantity.Should().Be(drink2.Model.Quantity);
            response.Model.Data[2].Quantity.Should().Be(drink3.Model.Quantity);
            response.Model.Data[3].Quantity.Should().Be(drink4.Model.Quantity);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Checkout.ShoppingService.Models;
using Checkout.ShoppingService.Repositories;
using NUnit.Framework;

namespace Checkout.ShoppingService.Tests
{
    [TestFixture]
    public class ShoppingRepositoryTests
    {
        private ShoppingRepository _repo;

        [SetUp]
        public void Setup()
        {
            _repo = new ShoppingRepository();
        }

        [TearDown]
        public void TearDown()
        {
            _repo.GetDrinksList().Clear();
        }

        [Test]
        public void Should_be_able_to_get_complete_drink_list()
        {
            var list = PopulateListWithDrinks();

            foreach (var item in list)
            {
                _repo.AddDrink(item);
            }

            var drinks = _repo.GetDrinksList();

            Assert.That(drinks.Count, Is.EqualTo(list.Count));
        }

        [TestCase("Evian", 30)]
        [TestCase("Beer", 55)]
        [TestCase("Coca Cola", 20)]
        [TestCase("Fanta", 15)]
        public void Should_get_drink_successfully(string name, int quantity)
        {
            var list = PopulateListWithDrinks();

            foreach (var item in list)
            {
                _repo.AddDrink(item);
            }

            var drink = _repo.GetDrink(name);
            
            Assert.That(drink.Quantity, Is.EqualTo(quantity));
        }

        [TestCase("Vodka", 30)]
        [TestCase("Mezcal", 55)]
        [TestCase("", 20)]
        public void Should_get_null_when_getting_drink_if_it_doesnt_exist(string name, int quantity)
        {
            var list = PopulateListWithDrinks();

            foreach (var item in list)
            {
                _repo.AddDrink(item);
            }

            var drink = _repo.GetDrink(name);

            Assert.That(drink, Is.Null);
        }

        [TestCase("Evian", 40)]
        [TestCase("Beer", 65)]
        [TestCase("Coca Cola", 2000)]
        [TestCase("Fanta", 150000)]
        public void Should_update_drink_successfully(string name, int quantity)
        {
            var list = PopulateListWithDrinks();

            foreach (var item in list)
            {
                _repo.AddDrink(item);
            }
            var drink = new Drink {Name = name, Quantity = quantity};
            _repo.UpdateDrink(drink);

            drink = _repo.GetDrink(name);

            Assert.That(drink.Quantity, Is.EqualTo(quantity));
        }

        [TestCase("", 10000, 5)]
        [TestCase("Apple Juice", 10, 5)]
        [TestCase("Snapple", 15, 5)]
        public void Should_not_update_if_drink_doesnt_exist(string name, int quantity, int expectedCount)
        {
            var list = PopulateListWithDrinks();

            foreach (var item in list)
            {
                _repo.AddDrink(item);
            }

            var drink = new Drink { Name = name, Quantity = quantity };

            _repo.UpdateDrink(drink);

            var drinks = _repo.GetDrinksList();

            Assert.That(drinks.Count, Is.EqualTo(expectedCount));
        }

        [TestCase("Apple Juice", 1)]
        [TestCase("Beer", 10)]
        public void Should_add_drink_successfully(string name, int quantity)
        {
            var drink = new Drink {Name = name, Quantity = quantity};
            _repo.AddDrink(drink);

            var list = _repo.GetDrinksList();

            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list.ElementAt(0).Name, Is.EqualTo(name));
            Assert.That(list.ElementAt(0).Quantity, Is.EqualTo(quantity));
        }

        [TestCase("Fanta", 4)]
        [TestCase("Beer", 4)]
        public void Should_delete_drink_successfully_if_exists(string name, int expectedCount)
        {
            var list = PopulateListWithDrinks();

            foreach (var item in list)
            {
                _repo.AddDrink(item);
            }

            _repo.DeleteDrink(name);

            var drinks = _repo.GetDrinksList();
            
            Assert.That(drinks.Count, Is.EqualTo(expectedCount));
        }

        [TestCase("", 5)]
        [TestCase("Apple Juice", 5)]
        [TestCase("Snapple", 5)]
        public void Should_not_delete_if_drink_doesnt_exist(string name, int expectedCount)
        {
            var list = PopulateListWithDrinks();

            foreach (var item in list)
            {
                _repo.AddDrink(item);
            }

            _repo.DeleteDrink(name);

            var drinks = _repo.GetDrinksList();

            Assert.That(drinks.Count, Is.EqualTo(expectedCount));
        }

        private static List<Drink> PopulateListWithDrinks()
        {
            return new List<Drink>
            {
                new Drink {Name = "Beer", Quantity = 55},
                new Drink {Name = "Fanta", Quantity = 15},
                new Drink {Name = "Coca Cola", Quantity = 20},
                new Drink {Name = "Orange Juice", Quantity = 20},
                new Drink {Name = "Evian", Quantity = 30},
            };
        }
    }
}

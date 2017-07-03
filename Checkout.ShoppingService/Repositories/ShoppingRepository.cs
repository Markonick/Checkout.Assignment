using System.Collections.Generic;
using System.Linq;
using Checkout.ShoppingService.Helpers;
using Checkout.ShoppingService.Models;

namespace Checkout.ShoppingService.Repositories
{
    public class ShoppingRepository : IShoppingRepository
    {
        private readonly List<Drink> _list;

        public ShoppingRepository()
        {
            _list = CreateShoppingList.ShoppingList;
        }

        public Drink GetDrink(string name)
        {
            var itemToReturn = _list.SingleOrDefault(item => item.Name == name);

            return itemToReturn != null ? new Drink { Name = itemToReturn.Name, Quantity = itemToReturn.Quantity } : null;
        }

        public List<Drink> GetDrinksList()
        {
            return _list;
        }

        public bool AddDrink(Drink drink)
        {
            var matches = _list.Where(list => list.Name == drink.Name);

            if (!matches.Any())
            {
                _list.Add(drink);
            }
            else
            {
                UpdateDrink(drink);
            }
            
            return true;
        }

        public bool UpdateDrink(Drink drink)
        {
            var itemToUpdate = _list.SingleOrDefault(item => item.Name == drink.Name);

            if (itemToUpdate == null) return false;

            itemToUpdate.Quantity = drink.Quantity;

            return true;
        }

        public bool DeleteDrink(string name)
        {
            var itemToRemove = _list.SingleOrDefault(item => item.Name == name);

            if (itemToRemove == null) return false;

            _list.Remove(itemToRemove);

            return true;
        }
    }
}
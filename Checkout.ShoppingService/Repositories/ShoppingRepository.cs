using System;
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

            return itemToReturn != null ? new Drink { Name = itemToReturn.Name, Quantity = itemToReturn.Quantity , DateCreated = itemToReturn.DateCreated} : null;
        }

        public List<Drink> GetDrinksList(string count, string offset, DateTime? fromDate, DateTime? toDate)
        {
            var query = _list.AsQueryable();
            
            if (fromDate != null && fromDate.Value.Date != DateTime.MinValue.Date)
            {
                query = query.Where(dto => dto.DateCreated >= fromDate.Value);
            }

            if (toDate != null && toDate.Value.Date != DateTime.MaxValue.Date)
            {
                query = query.Where(dto => dto.DateCreated <= toDate.Value);
            }

            var queryResult = query.OrderByDescending(dto => dto.Quantity).Skip(int.Parse(offset)).Take(int.Parse(count)).ToList();

            return queryResult;
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
            itemToUpdate.DateCreated = drink.DateCreated;

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
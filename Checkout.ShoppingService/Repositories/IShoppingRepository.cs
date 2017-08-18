using System;
using System.Collections.Generic;
using Checkout.ShoppingService.Models;

namespace Checkout.ShoppingService.Repositories
{
    public interface IShoppingRepository
    {
        bool AddDrink(Drink drink);
        bool DeleteDrink(string name);
        bool UpdateDrink(Drink drink);
        Drink GetDrink(string name);
        List<Drink> GetDrinksList(string count, string offset, DateTime? fromDate, DateTime? toDate);
    }
}
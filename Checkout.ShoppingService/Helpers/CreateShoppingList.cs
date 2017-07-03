using System.Collections.Generic;
using Checkout.ShoppingService.Models;

namespace Checkout.ShoppingService.Helpers
{
    public static class CreateShoppingList
    {
        public static List<Drink> ShoppingList { get; set; }

        static CreateShoppingList()
        {
            ShoppingList = new List<Drink>();
        }
    }
}

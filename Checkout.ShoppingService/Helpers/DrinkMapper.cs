using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkout.ShoppingService.Models;

namespace Checkout.ShoppingService.Helpers
{
    public static class DrinkMapper
    {
        public static Drink Map(DrinkModel drinkModel)
        {
            return new Drink
            {
                Name = drinkModel.Name,
                Quantity = int.Parse(drinkModel.Quantity),
                DateCreated = drinkModel.DateCreated
            };
        }
    }
}

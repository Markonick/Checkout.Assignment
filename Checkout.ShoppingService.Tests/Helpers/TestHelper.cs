using Checkout.ApiServices.Shopping.RequestModels;

namespace Checkout.ShoppingService.Tests.Helpers
{
    public class TestHelper
    {
        public static DrinkCreate GetDrinkCreateModel(string name, string quantity)
        {
            return new DrinkCreate
            {
                Name = name,
                Quantity = quantity
            };
        }

        public static DrinkUpdate GetDrinkUpdateModel(string name, string quantity)
        {
            return new DrinkUpdate
            {
                Name = name,
                Quantity = quantity
            };
        }
    }
}

using Checkout.ShoppingService.Models;

namespace Checkout.ShoppingService
{
    public interface IRequestValidator
    {
        string GetResult(DrinkModel input, bool validateBoth);
    }
}
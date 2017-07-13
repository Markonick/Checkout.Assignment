using System.Collections.Generic;
using Checkout.ApiServices.SharedModels;
using Checkout.ApiServices.Shopping.RequestModels;
using Checkout.ApiServices.Shopping.ResponseModels;
using Checkout.Utilities;

namespace Checkout.ApiServices.Shopping
{
    public class ShoppingService
    {
        public HttpResponse<DrinkList> GetShoppingList(DrinkGetList request)
        {
            var getDrinkListUri = ApiUrls.ShoppingList;

            if (request.Count.HasValue)
            {
                getDrinkListUri = UrlHelper.AddParameterToUrl(getDrinkListUri, "count", request.Count.ToString());
            }

            if (request.Offset.HasValue)
            {
                getDrinkListUri = UrlHelper.AddParameterToUrl(getDrinkListUri, "offset", request.Offset.ToString());
            }

            if (request.FromDate.HasValue)
            {
                getDrinkListUri = UrlHelper.AddParameterToUrl(getDrinkListUri, "fromDate", DateTimeHelper.FormatAsUtc(request.FromDate.Value));
            }

            if (request.ToDate.HasValue)
            {
                getDrinkListUri = UrlHelper.AddParameterToUrl(getDrinkListUri, "toDate", DateTimeHelper.FormatAsUtc(request.ToDate.Value));
            }

            return new ApiHttpClient().GetRequest<DrinkList>(getDrinkListUri, AppSettings.SecretKey);
        }

        public HttpResponse<DrinkCreate> AddDrink(DrinkCreate requestModel)
        {
            var addDrinkUri = string.Format(ApiUrls.ShoppingAddItem, requestModel.Name, requestModel.Quantity);
            return new ApiHttpClient().PostRequest<DrinkCreate>(addDrinkUri, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<DrinkModel> GetDrink(string name)
        {
            var getDrinkUri = string.Format(ApiUrls.ShoppingItem, name);
            return new ApiHttpClient().GetRequest<DrinkModel>(getDrinkUri, AppSettings.SecretKey);
        }

        public HttpResponse<OkResponse> UpdateDrink(DrinkUpdate requestModel)
        {
            var updateDrinkUri = string.Format(ApiUrls.ShoppingAddItem, requestModel.Name, requestModel.Quantity);
            return new ApiHttpClient().PutRequest<OkResponse>(updateDrinkUri, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<OkResponse> DeleteDrink(string name)
        {
            var deleteDrinkUri = string.Format(ApiUrls.ShoppingItem, name);
            return new ApiHttpClient().DeleteRequest<OkResponse>(deleteDrinkUri, AppSettings.SecretKey);
        }
    }
}

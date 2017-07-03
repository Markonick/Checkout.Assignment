using System;
using System.Collections.Generic;
using Checkout.ApiServices.Shopping.RequestModels;

namespace Checkout.ApiServices.Shopping.ResponseModels
{
    public class DrinkList
    {
        public int Count { get; set; }
        public List<DrinkModel> Data { get; set; }
    }
}
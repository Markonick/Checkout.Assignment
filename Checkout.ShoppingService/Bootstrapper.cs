using Checkout.ShoppingService.Models;
using Checkout.ShoppingService.Repositories;
using Checkout.ShoppingService.Validators;
using FluentValidation;
using Nancy;
using Nancy.TinyIoc;

namespace Checkout.ShoppingService
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        public static TinyIoCContainer Container;

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            container.Register<IShoppingRepository, ShoppingRepository>(new ShoppingRepository());
            container.Register<IRequestValidator, RequestValidator>(new RequestValidator(new FormatValidator(), new NameValidator(), new QuantityValidator()));

            Container = container;
        }
    }
}
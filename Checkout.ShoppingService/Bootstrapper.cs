using Checkout.ShoppingService.Repositories;
using Nancy;
using Nancy.Authentication.Basic;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace Checkout.ShoppingService
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        public static TinyIoCContainer Container;

        /*protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            pipelines.EnableBasicAuthentication(new BasicAuthenticationConfiguration(container.Resolve<IUserValidator>(), "MyShoppingAuth"));
        }*/

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            container.Register<IShoppingRepository, ShoppingRepository>(new ShoppingRepository());

            Container = container;
        }
    }
}
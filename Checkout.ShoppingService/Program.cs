using System;
using System.Configuration;
using Nancy.Hosting.Self;

namespace Checkout.ShoppingService
{
    public class Program
    {
        private static readonly string BaseUrl = ConfigurationManager.AppSettings["BaseUrl"];

        public static void Main(string[] args)
        {
            var configuration = new HostConfiguration
            {
                UrlReservations = new UrlReservations { CreateAutomatically = true }
            };
            
            using (var host = new NancyHost(configuration, new Uri(BaseUrl)))
            {
                host.Start();
                Console.WriteLine("Running on http://localhost:5000");
                Console.ReadKey();
                host.Stop();  // press a key to stop hosting
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Presentation
{
    using System.Net;
    using System.Reflection;

    using Application.Application;
    using Application.Infrastructure;

    using Autofac;
    using Autofac.Integration.WebApi;

    using EventStore.ClientAPI;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            RegisterProjectInterfaces(builder);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        private static void RegisterProjectInterfaces(ContainerBuilder builder)
        {
            var test = SetupEventStoreConnection();

            builder.RegisterInstance(test).As<IEventStoreConnection>().SingleInstance();
            builder.RegisterType<EventStore>().As<IEventStore>();
            builder.RegisterType<MoneyBoxRepository>().As<IMoneyBoxRepository>();
        }

        private static IEventStoreConnection SetupEventStoreConnection()
        {
            var connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            connection.ConnectAsync().Wait();
            return connection;
        }
    }
}

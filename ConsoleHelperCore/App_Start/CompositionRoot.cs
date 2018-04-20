using System;
using System.Collections.Generic;
using System.Text;
using ConsoleHelperCore.Diagnostics.Entities;
using ConsoleHelperCore.Diagnostics.Shared;
using Microsoft.AspNet.SignalR;
using Microsoft.Extensions.Configuration;
using SimpleInjector;

namespace ConsoleHelperCore.App_Start
{
    public static class CompositionRoot
    {
        public static void Initialize()
        {
            InitializeContainer();
        }

        private static void InitializeContainer()
        {
            var container = new Container();
            RegisterServices(container);

            container.Verify();

            var resolver = new SimpleInjectorResolver(container);
            GlobalHost.DependencyResolver = resolver;
        }

        private static void RegisterServices(Container container)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var appInsightsOptions = new AppInsightsOptions { LogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), config.GetValue<string>("LogLevel")), InstrumentationKey = config.GetValue<string>("InstrumentationKey") };
            container.AddApplicationInsightsUsingSimpleInjector().AddAppInsightsLoggingOptions(appInsightsOptions);
        }
    }
}

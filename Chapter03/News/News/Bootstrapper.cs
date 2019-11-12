using System;
using System.Reflection;
using Autofac;
using News.ViewModels;

namespace News
{
    public static class Bootstrapper
    {
        public static void Initialize()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<MainShell>();

            containerBuilder.RegisterType<Navigator>().As<INavigate>();

            containerBuilder.RegisterAssemblyTypes(typeof(App).Assembly).Where(x => x.IsSubclassOf(typeof(ViewModel)));

            var container = containerBuilder.Build();

            Resolver.Initialize(container);
        }
    }
}

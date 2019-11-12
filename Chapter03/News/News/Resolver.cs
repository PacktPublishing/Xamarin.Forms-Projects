using System;
using System.Runtime.CompilerServices;
using Autofac;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace News
{
    public static class Resolver
    {
        private static IContainer container;

        public static void Initialize(IContainer container)
        {
            Resolver.container = container;
        }

        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}

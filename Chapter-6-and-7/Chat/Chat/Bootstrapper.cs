using Autofac;
using Chat.Chat;
using System;
using System.Reflection;

namespace Chat
{
    public class Bootstrapper
    {
        public static void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ChatService>().As<IChatService>().SingleInstance();

            var currentAssembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(currentAssembly)
                     .Where(x => x.Name.EndsWith("View", StringComparison.Ordinal));

            builder.RegisterAssemblyTypes(currentAssembly)
                    .Where(x => x.Name.EndsWith("ViewModel", StringComparison.Ordinal));

            var container = builder.Build();

            Resolver.Initialize(container);
        }
    }
}

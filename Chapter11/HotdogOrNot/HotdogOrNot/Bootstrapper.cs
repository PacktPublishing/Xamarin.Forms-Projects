using Autofac;
using HotdogOrNot.ViewModels;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace HotdogOrNot
{
    public class Bootstrapper
    {
        protected ContainerBuilder ContainerBuilder { get; private set; }

        public Bootstrapper()
        {
            Initialize();
            FinishInitialization();
        }

        protected virtual void Initialize()
        {
            ContainerBuilder = new ContainerBuilder();

            var currentAssembly = Assembly.GetExecutingAssembly();

            foreach (var type in currentAssembly.DefinedTypes.Where(e => e.IsSubclassOf(typeof(Page))))
            {
                ContainerBuilder.RegisterType(type.AsType());
            }

            foreach (var type in currentAssembly.DefinedTypes.Where(e => e.IsSubclassOf(typeof(ViewModel))))
            {
                ContainerBuilder.RegisterType(type.AsType());
            }
        }

        private void FinishInitialization()
        {
            var container = ContainerBuilder.Build();

            Resolver.Initialize(container);
        }
    }
}

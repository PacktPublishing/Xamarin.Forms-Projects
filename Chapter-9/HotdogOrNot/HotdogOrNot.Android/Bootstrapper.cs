using Autofac;

namespace HotdogOrNot.Droid
{
    public class Bootstrapper : HotdogOrNot.Bootstrapper
    {
        public static void Init()
        {
            var instance = new Bootstrapper();
        }

        protected override void Initialize()
        {
            base.Initialize();

            ContainerBuilder.RegisterType<TensorflowClassifier>().As<IClassifier>().SingleInstance();
        }
    }
}
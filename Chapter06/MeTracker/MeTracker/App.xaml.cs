using MeTracker.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MeTracker
{
    public partial class App : Application
    {
        private bool isStartUp;

        public App()
        {
            InitializeComponent();

            MainPage = Resolver.Resolve<MainView>();

            isStartUp = false;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            if(!isStartUp)
            {
                MainPage = Resolver.Resolve<MainView>();
            }
            
        }
    }
}

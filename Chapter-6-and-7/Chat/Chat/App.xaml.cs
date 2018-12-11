using Chat.Chat;
using Chat.ViewModels;
using Chat.Views;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Chat
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Bootstrapper.Init();

            var mainView = Resolver.Resolve<MainView>();
            var navigationPage = new NavigationPage(mainView);
            ViewModel.Navigation = navigationPage.Navigation;
            MainPage = navigationPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            var chatService = Resolver.Resolve<IChatService>();
            chatService.Dispose();
        }

        protected override void OnResume()
        {
            Task.Run(async () =>
            {
                var chatService = Resolver.Resolve<IChatService>();

                if (!chatService.IsConnected)
                {
                    await chatService.CreateConnection();
                }
            });

            Page view = null;

            if (ViewModel.User != null)
            {
                view = Resolver.Resolve<ChatView>();
            }
            else
            {
                view = Resolver.Resolve<MainView>();
            }

            var navigationPage = new NavigationPage(view);
            MainPage = navigationPage;
        }
    }
}

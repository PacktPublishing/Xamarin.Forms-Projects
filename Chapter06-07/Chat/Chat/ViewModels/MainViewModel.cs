using Chat.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace Chat.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public string Username { get; set; }

        public ICommand Start => new Command(() =>
        {
            User = Username;

            var chatView = Resolver.Resolve<ChatView>();
            Navigation.PushAsync(chatView);
        });
    }
}

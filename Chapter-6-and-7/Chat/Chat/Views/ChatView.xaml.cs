using Chat.ViewModels;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace Chat.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatView : ContentPage
	{
        private ChatViewModel viewModel;

        public ChatView (ChatViewModel viewModel)
		{
            this.viewModel = viewModel;

			InitializeComponent ();

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            viewModel.Messages.CollectionChanged += Messages_CollectionChanged;
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var safeArea = On<Xamarin.Forms.PlatformConfiguration.iOS>().SafeAreaInsets();
            MainGrid.HeightRequest = this.Height - safeArea.Top - safeArea.Bottom;
        }

        private void Messages_CollectionChanged(object sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            MessageList.ScrollTo(viewModel.Messages.Last(), ScrollToPosition.End, true);
        }
    }
}
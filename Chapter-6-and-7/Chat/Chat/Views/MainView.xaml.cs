using Chat.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace Chat.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainView : ContentPage
	{
        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            BindingContext = viewModel;
        }
    }
}
using MeTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MeTracker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainView : ContentPage
	{
		public MainView (MainViewModel viewModel)
		{
			InitializeComponent ();

            BindingContext = viewModel;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if(location == null)
                {
                    location = await Geolocation.GetLocationAsync();
                }

                Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(5)));
            });
        }
	}
}
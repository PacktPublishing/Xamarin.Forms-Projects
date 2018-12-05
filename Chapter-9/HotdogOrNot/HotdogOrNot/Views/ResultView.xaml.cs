using HotdogOrNot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HotdogOrNot.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResultView : ContentPage
	{
		public ResultView (ResultViewModel viewModel)
		{
			InitializeComponent ();

            BindingContext = viewModel;
		}
	}
}
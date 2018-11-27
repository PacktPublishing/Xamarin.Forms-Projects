using DoToo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoToo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemView : ContentPage
	{
		public ItemView (ItemViewModel viewmodel)
		{
			InitializeComponent ();
            viewmodel.Navigation = Navigation;
            BindingContext = viewmodel;
        }
	}
}
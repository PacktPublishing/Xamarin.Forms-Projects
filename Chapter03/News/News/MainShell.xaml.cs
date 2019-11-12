using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using News.ViewModels;
using Xamarin.Forms;

namespace News
{
    public partial class MainShell
    {
        public MainShell(MainViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }

    public class Navigator : INavigate
    {
        public async Task NavigateTo(string route)
        {
            await Shell.Current.GoToAsync(route);
        }
    }
}

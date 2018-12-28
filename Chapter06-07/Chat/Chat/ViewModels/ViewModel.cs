using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Chat.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public static INavigation Navigation { get; set; }
        public static string User { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Set<T>(ref T field, T newValue,
                              [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

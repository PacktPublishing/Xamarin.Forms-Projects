using System;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;

namespace News.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        public INavigate Navigation { get; set; }
    }

    public interface INavigate
    {
        Task NavigateTo(string route);
    }
}

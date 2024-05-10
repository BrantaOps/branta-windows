using System.ComponentModel;
using System.Windows.Threading;

namespace Branta.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public Dispatcher DispatchHelper => System.Windows.Application.Current.Dispatcher;

    protected void OnPropertyChanged(string properyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(properyName));
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace Branta.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public Dispatcher DispatchHelper => System.Windows.Application.Current.Dispatcher;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

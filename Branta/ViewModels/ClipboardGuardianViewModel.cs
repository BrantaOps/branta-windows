using Branta.Classes;
using Branta.Commands;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace Branta.ViewModels;

public class ClipboardGuardianViewModel : BaseViewModel
{
    private readonly Timer _clipboardGuardianTimer;

    private ClipboardItemViewModel _clipboardItem;
    public ClipboardItemViewModel ClipboardItem
    {
        get => _clipboardItem;
        set
        {
            _clipboardItem = value;
            OnPropertyChanged();
        }
    }

    public ICommand ClipboardGuardianCommand { get; }

    public ClipboardGuardianViewModel(NotificationCenter notificationCenter, Settings settings, ResourceDictionary resourceDictionary)
    {
        ClipboardGuardianCommand = new ClipboardGuardianCommand(this, notificationCenter, settings, resourceDictionary);

        _clipboardGuardianTimer = new Timer(new TimeSpan(0, 0, 1));
        _clipboardGuardianTimer.Elapsed += (object sender, ElapsedEventArgs e) => ClipboardGuardianCommand.Execute(null);
        _clipboardGuardianTimer.Start();
    }
}

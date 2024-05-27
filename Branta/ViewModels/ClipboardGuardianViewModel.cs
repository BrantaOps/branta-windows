using Branta.Commands;
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

    public ClipboardGuardianViewModel(ClipboardGuardianCommand clipboardGuardianCommand)
    {
        ClipboardGuardianCommand = clipboardGuardianCommand;

        _clipboardGuardianTimer = new Timer(new TimeSpan(0, 0, 0, 0, 500));
        _clipboardGuardianTimer.Elapsed += (_, _) => ClipboardGuardianCommand.Execute(this);
        _clipboardGuardianTimer.Start();
    }
}

using Branta.Commands;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace Branta.ViewModels;

public partial class ClipboardGuardianViewModel : ObservableObject
{
    private readonly Timer _clipboardGuardianTimer;

    [ObservableProperty]
    private ClipboardItemViewModel _clipboardItem;

    public ICommand ClipboardGuardianCommand { get; }

    public ClipboardGuardianViewModel(ClipboardGuardianCommand clipboardGuardianCommand)
    {
        ClipboardGuardianCommand = clipboardGuardianCommand;

        _clipboardGuardianTimer = new Timer(new TimeSpan(0, 0, 0, 0, milliseconds: 500));
        _clipboardGuardianTimer.Elapsed += (_, _) => ClipboardGuardianCommand.Execute(this);
        _clipboardGuardianTimer.Start();
    }
}

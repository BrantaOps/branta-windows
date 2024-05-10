using Branta.Classes;
using Branta.Commands;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace Branta.ViewModels;

public class MainViewModel
{
    private readonly Timer _updateAppTimer;
    private readonly Timer _clipboardGuardianTimer;

    public InstallerVerificationViewModel InstallerVerificationViewModel { get; set; }
    public WalletVerificationViewModel WalletVerificationViewModel { get; set; }

    public ICommand UpdateAppCommand { get; }
    public ICommand ClipboardGuardianCommand { get; }

    public MainViewModel(
        InstallerVerificationViewModel installerVerificationViewModel,
        WalletVerificationViewModel walletVerificationViewModel,
        NotificationCenter notificationCenter,
        ResourceDictionary resourceDictionary,
        Settings settings)
    {
        InstallerVerificationViewModel = installerVerificationViewModel;
        WalletVerificationViewModel = walletVerificationViewModel;

        UpdateAppCommand = new UpdateAppCommand(notificationCenter, resourceDictionary);
        ClipboardGuardianCommand = new ClipboardGuardianCommand(notificationCenter, settings);

        _updateAppTimer = new Timer(new TimeSpan(24, 0, 0));
        _updateAppTimer.Elapsed += (object sender, ElapsedEventArgs e) => UpdateAppCommand.Execute(null);
        _updateAppTimer.Start();

        UpdateAppCommand.Execute(null);

        _clipboardGuardianTimer = new Timer(new TimeSpan(0, 0, 5));
        _clipboardGuardianTimer.Elapsed += (object sender, ElapsedEventArgs e) => ClipboardGuardianCommand.Execute(null);
        _clipboardGuardianTimer.Start();
    }
}

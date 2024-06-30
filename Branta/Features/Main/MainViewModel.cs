using Branta.Commands;
using Branta.Features.ClipboardGuardian;
using Branta.Features.InstallerVerification;
using Branta.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace Branta.Features.Main;

public class MainViewModel : ObservableObject
{
    private readonly Timer _updateAppTimer;

    public InstallerVerificationViewModel InstallerVerificationViewModel { get; set; }
    public WalletVerificationViewModel WalletVerificationViewModel { get; set; }
    public ClipboardGuardianViewModel ClipboardGuardianViewModel { get; set; }

    public ICommand UpdateAppCommand { get; }
    public ICommand HelpCommand { get; }

    public MainViewModel(
        InstallerVerificationViewModel installerVerificationViewModel,
        WalletVerificationViewModel walletVerificationViewModel,
        ClipboardGuardianViewModel clipboardGuardianViewModel,
        UpdateAppCommand updateAppCommand)
    {
        InstallerVerificationViewModel = installerVerificationViewModel;
        WalletVerificationViewModel = walletVerificationViewModel;
        ClipboardGuardianViewModel = clipboardGuardianViewModel;

        UpdateAppCommand = updateAppCommand;
        HelpCommand = new HelpCommand();

        _updateAppTimer = new Timer(new TimeSpan(24, 0, 0));
        _updateAppTimer.Elapsed += (_, _) => UpdateAppCommand.Execute(null);
        _updateAppTimer.Start();

        UpdateAppCommand.Execute(null);
    }
}
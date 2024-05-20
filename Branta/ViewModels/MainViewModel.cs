﻿using Branta.Classes;
using Branta.Commands;
using System.Windows;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace Branta.ViewModels;

public class MainViewModel
{
    private readonly Timer _updateAppTimer;

    public InstallerVerificationViewModel InstallerVerificationViewModel { get; set; }
    public WalletVerificationViewModel WalletVerificationViewModel { get; set; }
    public ClipboardGuardianViewModel ClipboardGuardianViewModel { get; set; }

    public ICommand UpdateAppCommand { get; }

    public MainViewModel(
        InstallerVerificationViewModel installerVerificationViewModel,
        WalletVerificationViewModel walletVerificationViewModel,
        ClipboardGuardianViewModel clipboardGuardianViewModel,
        NotificationCenter notificationCenter,
        ResourceDictionary resourceDictionary,
        Settings settings)
    {
        InstallerVerificationViewModel = installerVerificationViewModel;
        WalletVerificationViewModel = walletVerificationViewModel;
        ClipboardGuardianViewModel = clipboardGuardianViewModel;

        UpdateAppCommand = new UpdateAppCommand(notificationCenter, resourceDictionary);

        _updateAppTimer = new Timer(new TimeSpan(24, 0, 0));
        _updateAppTimer.Elapsed += (_, _) => UpdateAppCommand.Execute(null);
        _updateAppTimer.Start();

        UpdateAppCommand.Execute(null);
    }
}
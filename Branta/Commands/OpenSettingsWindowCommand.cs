using Branta.Classes;
using Branta.ViewModels;
using Branta.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Branta.Commands;

public class OpenSettingsWindowCommand : BaseCommand
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Settings _settings;
    private readonly WalletVerificationViewModel _walletVerificationViewModel;

    public OpenSettingsWindowCommand(IServiceProvider serviceProvider, Settings settings,
        WalletVerificationViewModel walletVerificationViewModel)
    {
        _serviceProvider = serviceProvider;
        _settings = settings;
        _walletVerificationViewModel = walletVerificationViewModel;
    }

    public override void Execute(object parameter)
    {
        var settingsWindow = _serviceProvider.GetRequiredService<SettingsWindow>();

        settingsWindow.ShowDialog();

        var settings = settingsWindow.GetSettings();

        if (settings.WalletVerification.WalletVerifyEvery != _settings.WalletVerification.WalletVerifyEvery)
        {
            _walletVerificationViewModel.SetTimer(settings.WalletVerification.WalletVerifyEvery);
        }

        _settings.Update(settings);
    }
}
using Branta.Classes;
using Branta.ViewModels;
using System.Windows;

namespace Branta;

public partial class App
{
    private readonly NotificationCenter _notificationCenter;
    private readonly Settings _settings;
    private readonly ResourceDictionary _resourceDictionary;

    public App()
    {
        _notificationCenter = new NotificationCenter();
        _settings = Settings.Load();
        _resourceDictionary = BaseWindow.GetLanguageDictionary();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var installerVerificationViewModel = new InstallerVerificationViewModel(_notificationCenter, _resourceDictionary);
        var walletVerificationViewModel = new WalletVerificationViewModel(_notificationCenter, _settings, _resourceDictionary);
        var clipboardGuardianViewModel = new ClipboardGuardianViewModel(_notificationCenter, _settings);

        MainWindow = new MainWindow(_notificationCenter, _settings, walletVerificationViewModel)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            DataContext = new MainViewModel(installerVerificationViewModel, walletVerificationViewModel, clipboardGuardianViewModel, _notificationCenter, _resourceDictionary, _settings)
        };
        MainWindow.Show();

        base.OnStartup(e);
    }
}
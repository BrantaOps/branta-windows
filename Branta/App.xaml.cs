using Branta.Classes;
using Branta.Stores;
using Branta.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace Branta;

public partial class App
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<NotificationCenter>();
                services.AddSingleton(Settings.Load());
                services.AddSingleton(BaseWindow.GetLanguageDictionary());
                services.AddSingleton<CheckSumStore>();

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<InstallerVerificationViewModel>();
                services.AddSingleton<WalletVerificationViewModel>();
                services.AddSingleton<ClipboardGuardianViewModel>();

                services.AddSingleton(s => new MainWindow(s.GetRequiredService<NotificationCenter>(),
                    s.GetRequiredService<Settings>(), s.GetRequiredService<ResourceDictionary>(),
                    s.GetRequiredService<WalletVerificationViewModel>(), s.GetRequiredService<CheckSumStore>())
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    DataContext = s.GetRequiredService<MainViewModel>()
                });
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _host.Start();

        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host.Dispose();

        base.OnExit(e);
    }
}
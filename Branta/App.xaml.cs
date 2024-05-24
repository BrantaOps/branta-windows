using Branta.Classes;
using Branta.Stores;
using Branta.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using Branta.Commands;
using Microsoft.Extensions.Configuration;

namespace Branta;

public partial class App
{
    private readonly IHost _host;

    private const int SW_RESTORE = 9;
    private const uint WM_SHOWWINDOW = 0x0018;
    private const int SW_PARENTOPENING = 3;

    [DllImport("user32.dll")]
    static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("Properties/appsettings.json").Build();
                var section = config.GetSection("AppSettings");
                var appSettings = section.Get<AppSettings>();
                services.AddSingleton(appSettings);

                services.AddSingleton<NotificationCenter>();
                services.AddSingleton(Settings.Load());
                services.AddSingleton(BaseWindow.GetLanguageDictionary());

                services.AddSingleton<CheckSumStore>();

                services.AddSingleton<ClipboardGuardianCommand>();
                services.AddSingleton<FocusCommand>();
                services.AddSingleton<LoadCheckSumsCommand>();
                services.AddSingleton<UpdateAppCommand>();
                services.AddSingleton<VerifyWalletsCommand>();

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<InstallerVerificationViewModel>();
                services.AddSingleton<WalletVerificationViewModel>();
                services.AddSingleton<ClipboardGuardianViewModel>();

                services.AddSingleton(s => new MainWindow(s.GetRequiredService<NotificationCenter>(),
                    s.GetRequiredService<Settings>(), s.GetRequiredService<ResourceDictionary>(),
                    s.GetRequiredService<WalletVerificationViewModel>(), s.GetRequiredService<CheckSumStore>(),
                    s.GetRequiredService<AppSettings>())
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

        try
        {
            var runningProcess = GetRunningProcess();

            if (runningProcess != null)
            {
                if (runningProcess.MainWindowHandle != IntPtr.Zero)
                {
                    ShowWindowAsync(runningProcess.MainWindowHandle, SW_RESTORE);
                    SetForegroundWindow(runningProcess.MainWindowHandle);
                    SendMessage(runningProcess.MainWindowHandle, WM_SHOWWINDOW, IntPtr.Zero, new IntPtr(SW_PARENTOPENING));

                    Current.Shutdown();
                    return;
                }
            }
        }
        catch
        {
            // ignored
        }

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host.Dispose();

        base.OnExit(e);
    }

    private static Process GetRunningProcess()
    {
        var currentProcess = Process.GetCurrentProcess();

        return Process
            .GetProcesses()
            .FirstOrDefault(p => p.Id != currentProcess.Id &&
                        p.ProcessName.Equals(currentProcess.ProcessName, StringComparison.Ordinal));
    }
}
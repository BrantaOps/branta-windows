using Branta.Classes;
using Branta.Commands;
using Branta.Stores;
using Branta.ViewModels;
using Branta.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

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
            .UseSerilog((host, loggerConfiguration) =>
            {
                var levelSwitch = new LoggingLevelSwitch
                {
                    MinimumLevel = Debugger.IsAttached ? LogEventLevel.Debug : LogEventLevel.Error
                };

                var outputTemplate =
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{MachineName}] [{SourceContext}] {Message:lj}{NewLine}{Exception}";

                loggerConfiguration.WriteTo
                    .File(FileStorage.GetBrantaDataPath("logs", "log_.txt"), rollingInterval: RollingInterval.Day,
                        outputTemplate: outputTemplate)
                    .WriteTo.Debug(outputTemplate: outputTemplate)
                    .MinimumLevel.ControlledBy(levelSwitch);
            })
            .ConfigureServices(services =>
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("Properties/appsettings.json")
                    .AddJsonFile("Properties/appsettings.Secrets.json")
                    .Build();
                var section = config.GetSection("AppSettings");
                var appSettings = section.Get<AppSettings>();
                services.AddSingleton(appSettings);

                services.AddSingleton<NotificationCenter>();
                services.AddSingleton(Settings.Load());
                services.AddSingleton(LanguageStore.Load());

                services.AddSingleton<CheckSumStore>();
                services.AddSingleton<InstallerHashStore>();

                services.AddSingleton<ClipboardGuardianCommand>();
                services.AddSingleton<FocusCommand>();
                services.AddSingleton<LoadCheckSumsCommand>();
                services.AddSingleton<LoadInstallerHashesCommand>();
                services.AddSingleton<UpdateAppCommand>();
                services.AddSingleton<VerifyWalletsCommand>();

                services.AddSingleton<NetworkActivityWindow>();

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<InstallerVerificationViewModel>();
                services.AddSingleton<WalletVerificationViewModel>();
                services.AddSingleton<ClipboardGuardianViewModel>();

                services.AddSingleton(s => new MainWindow(s.GetRequiredService<NotificationCenter>(),
                    s.GetRequiredService<Settings>(), s.GetRequiredService<LanguageStore>(),
                    s.GetRequiredService<WalletVerificationViewModel>(), s.GetRequiredService<CheckSumStore>(),
                    s.GetRequiredService<InstallerHashStore>(), s.GetRequiredService<InstallerVerificationViewModel>(),
                    s.GetRequiredService<AppSettings>(), s.GetRequiredService<ILogger<MainWindow>>())
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
                    SendMessage(runningProcess.MainWindowHandle, WM_SHOWWINDOW, IntPtr.Zero,
                        new IntPtr(SW_PARENTOPENING));

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
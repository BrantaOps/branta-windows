using Branta.Classes;
using Branta.Commands;
using Branta.Core.Data;
using Branta.Stores;
using Branta.ViewModels;
using Branta.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

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

                services.AddSingleton<CheckSumStore>();
                services.AddSingleton<ExtendedKeyStore>();
                services.AddSingleton<InstallerHashStore>();
                services.AddSingleton(LanguageStore.Load());

                services.AddSingleton<ClipboardGuardianCommand>();
                services.AddSingleton<FocusCommand>();
                services.AddSingleton<UpdateAppCommand>();
                services.AddSingleton<VerifyWalletsCommand>();

                services.AddSingleton<ClipboardGuardianViewModel>();
                services.AddSingleton<ExtendedKeyManagerViewModel>();
                services.AddSingleton<InstallerVerificationViewModel>();
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<SettingsViewModel>();
                services.AddSingleton<WalletVerificationViewModel>();

                services.AddDbContext<BrantaContext>(options =>
                {
                    options.UseSqlite($"Data Source={Path.Join(FileStorage.GetBrantaDataPath("BrantaCore.db"))}");
                });

                services.AddTransient<SettingsWindow>();

                services.AddSingleton(s =>
                    new MainWindow(s.GetRequiredService<LanguageStore>(), s.GetRequiredService<AppSettings>(),
                        s.GetRequiredService<ILogger<MainWindow>>())
                    {
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        DataContext = s.GetRequiredService<MainViewModel>()
                    });
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        try
        {
            var runningProcess = GetRunningProcess();

            if (runningProcess != null)
            {
                var mainWindowHandle = runningProcess.MainWindowHandle != IntPtr.Zero
                    ? runningProcess.MainWindowHandle
                    : GetMainWindowHandle(runningProcess.Id);

                if (mainWindowHandle != IntPtr.Zero)
                {
                    ShowWindowAsync(mainWindowHandle, SW_RESTORE);
                    SetForegroundWindow(mainWindowHandle);
                    SendMessage(mainWindowHandle, WM_SHOWWINDOW, IntPtr.Zero,
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

        _host.Start();

        using var scope = _host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var brantaContext = scope.ServiceProvider.GetRequiredService<BrantaContext>();
        brantaContext.Database.Migrate();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();

        var notificationCenter = _host.Services.GetRequiredService<NotificationCenter>();

        var extendedKeyStore = _host.Services.GetRequiredService<ExtendedKeyStore>();
        Task.Run(extendedKeyStore.LoadAsync);

        notificationCenter.Setup(mainWindow);

        if (!Environment.GetCommandLineArgs().Contains("headless"))
        {
            mainWindow.Show();
        }

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

    private static IntPtr GetMainWindowHandle(int processId)
    {
        var mainWindowHandle = IntPtr.Zero;

        var process = Process.GetProcessById((int)processId);

        EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
        {
            GetWindowThreadProcessId(hWnd, out var windowProcessId);

            if (windowProcessId == processId)
            {
                var windowText = new StringBuilder(256);
                GetWindowText(hWnd, windowText, windowText.Capacity);

                if (string.Equals(windowText.ToString(), "Branta", StringComparison.CurrentCultureIgnoreCase))
                {
                    mainWindowHandle = hWnd;
                    return false;
                }
            }

            return true;
        }, IntPtr.Zero);

        return mainWindowHandle;
    }
}
using Branta.Classes;
using Branta.Stores;
using Branta.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using Branta.Commands;

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

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("user32.dll")]
    private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

    private static IntPtr FindWindowByTitle(string title)
    {
        var foundHandle = IntPtr.Zero;

        EnumWindows((hWnd, lParam) =>
        {
            var windowText = new StringBuilder(256);
            GetWindowText(hWnd, windowText, windowText.Capacity);

            if (windowText.ToString() == title)
            {
                GetWindowThreadProcessId(hWnd, out var processId);
                var process = Process.GetProcessById((int)processId);

                if (process.MainModule!.ModuleName == "Branta.exe")
                {
                    foundHandle = hWnd;
                    return false;
                }
            }

            return true;
        }, IntPtr.Zero);

        return foundHandle;
    }

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<NotificationCenter>();
                services.AddSingleton(Settings.Load());
                services.AddSingleton(BaseWindow.GetLanguageDictionary());
                services.AddSingleton<CheckSumStore>();

                services.AddSingleton<ClipboardGuardianCommand>();
                services.AddSingleton<FocusCommand>();
                services.AddSingleton<LoadCheckSumsCommand>();
                services.AddSingleton<VerifyWalletsCommand>();

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

        try
        {
            if (GetRunningProcess() != null)
            {
                var existingWindowHandle = FindWindowByTitle("Branta");

                if (existingWindowHandle != IntPtr.Zero)
                {
                    ShowWindowAsync(existingWindowHandle, SW_RESTORE);
                    SetForegroundWindow(existingWindowHandle);
                    SendMessage(existingWindowHandle, WM_SHOWWINDOW, IntPtr.Zero, new IntPtr(SW_PARENTOPENING));

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
            .FirstOrDefault(p => p.Id == currentProcess.Id &&
                                 p.ProcessName.Equals(currentProcess.ProcessName, StringComparison.Ordinal));
    }
}
using Branta.Classes;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Branta.Automation;

public abstract class BaseAutomation : DispatcherObject
{
    protected readonly NotifyIcon NotifyIcon;

    protected Settings Settings;

    public int RunInterval { get; set; }

    private bool _processingComplete = true;

    protected BaseAutomation(NotifyIcon notifyIcon, Settings settings, int runInterval)
    {
        NotifyIcon = notifyIcon;
        RunInterval = runInterval;
        Settings = settings;
    }

    public abstract void Run();

    public void Elapsed(object sender, ElapsedEventArgs e)
    {
        if (!_processingComplete)
        {
            return;
        }

        _processingComplete = false;

        Task.Run(Run)
            .ContinueWith(_ =>
            {
                _processingComplete = true;
            });
    }

    public System.Timers.Timer CreateTimer()
    {
        var timer = new System.Timers.Timer(RunInterval * 1000);

        timer.Elapsed += Elapsed;
        timer.Start();

        return timer;
    }

    public void SubscribeToSettingsChanges(MainWindow mainWindow)
    {
        mainWindow.SettingsChanged += UpdateSettings;
    }

    private void UpdateSettings(Settings newSettings)
    {
        Settings = newSettings;
    }
}

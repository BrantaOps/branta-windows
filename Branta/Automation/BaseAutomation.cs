using System.Diagnostics;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Branta.Automation;

public abstract class BaseAutomation : DispatcherObject
{
    protected readonly NotifyIcon NotifyIcon;

    public int RunInterval { get; }

    private bool _processingComplete = true;

    protected BaseAutomation(NotifyIcon notifyIcon, int runInterval)
    {
        NotifyIcon = notifyIcon;
        RunInterval = runInterval;
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
}

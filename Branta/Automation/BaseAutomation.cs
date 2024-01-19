using System.Timers;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Branta.Automation;

public abstract class BaseAutomation : DispatcherObject
{
    protected readonly NotifyIcon NotifyIcon;

    public abstract int RunInterval { get; }

    protected BaseAutomation(NotifyIcon notifyIcon)
    {
        NotifyIcon = notifyIcon;
    }

    public abstract void Run();

    public abstract void Update();

    public void Elapsed(object sender, ElapsedEventArgs e)
    {
        Task.Run(Run)
            .ContinueWith(_ =>
            {
                Dispatcher.Invoke(Update);
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

using System.Timers;
using System.Windows.Threading;

namespace Branta.Automation;

public abstract class BaseAutomation : DispatcherObject
{
    public abstract int RunInterval { get; }

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
}

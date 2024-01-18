using System.Windows.Threading;

namespace Branta.Automation;

public abstract class BaseAutomation : DispatcherObject
{
    public abstract int RunInterval { get; }

    public abstract void Run();
}

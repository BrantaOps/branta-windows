using System.Windows.Forms;

namespace Branta.Classes;

public class Notification
{
    public string Title { get; set; } = "Branta";

    public string Message { get; set; }

    public ToolTipIcon Icon { get; set; } = ToolTipIcon.Info;

    public int Timeout { get; set; } = 3000;
}

using System.Windows.Forms;

namespace Branta.Classes;

public static class BrantaExtensions
{
    public static void ShowBalloonTip(this NotifyIcon notifyIcon, Notification notification)
    {
        notifyIcon.ShowBalloonTip(notification.Timeout, notification.Title, notification.Message, notification.Icon);
    }
}

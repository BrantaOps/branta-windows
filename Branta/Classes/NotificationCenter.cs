using System.Drawing;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace Branta.Classes;

public class NotificationCenter
{
    private readonly NotifyIcon _notifyIcon;

    public NotifyIcon NotifyIcon => _notifyIcon;

    public NotificationCenter()
    {
        _notifyIcon = new NotifyIcon
        {
            Icon = new Icon(Application.GetResourceStream(new Uri("pack://application:,,,/Assets/black_circle.ico"))!.Stream),
            Text = "Branta",
            Visible = true
        };
    }

    public void Notify(Notification notification)
    {
        _notifyIcon.ShowBalloonTip(notification.Timeout, notification.Title, notification.Message ?? " ", notification.Icon);
    }
}

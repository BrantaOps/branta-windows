using System.Collections.ObjectModel;
using System.Windows.Forms;
using Branta.Domain;

namespace Branta.Classes;

public static class BrantaExtensions
{
    public static void ShowBalloonTip(this NotifyIcon notifyIcon, Notification notification)
    {
        notifyIcon.ShowBalloonTip(notification.Timeout, notification.Title, notification.Message, notification.Icon);
    }

    public static ObservableCollection<Wallet> Set(this ObservableCollection<Wallet> wallets, List<Wallet> newWallets)
    {
        wallets.Clear();
        foreach (var wallet in newWallets)
        {
            wallets.Add(wallet);
        }

        return wallets;
    }
}

using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
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

    public static void SetSource(this System.Windows.Controls.Image image, string path)
    {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri($"pack://application:,,,/{path}", UriKind.Absolute);
            bitmapImage.EndInit();

            image.Source = bitmapImage;
    }
}

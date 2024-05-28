using System.Windows.Media.Imaging;

namespace Branta.Classes;

public static class BrantaExtensions
{
    public static void SetSource(this System.Windows.Controls.Image image, string path)
    {
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.UriSource = new Uri($"pack://application:,,,/{path}", UriKind.Absolute);
        bitmapImage.EndInit();

        image.Source = bitmapImage;
    }

    public static string Format(this TimeSpan timeSpan)
    {
        if (timeSpan.Hours > 0)
        {
            return $"{timeSpan.Hours}h";
        }

        if (timeSpan.Minutes > 0)
        {
            return $"{timeSpan.Minutes}m";
        }

        return $"{timeSpan.Seconds}s";
    }
}
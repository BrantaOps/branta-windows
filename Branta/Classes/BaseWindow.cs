using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Branta.Classes;

public class BaseWindow : Window
{
    private System.Windows.Controls.Image _resizeImage;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void  OnPropertyChanged([CallerMemberName] string name = "")
    {
        var handler = PropertyChanged;
        handler?.Invoke(this, new  PropertyChangedEventArgs(name));
    }

    public void SetResizeImage(System.Windows.Controls.Image resizeImage)
    {
        _resizeImage = resizeImage;
    }

    protected void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        try
        {
            DragMove();
        }
        catch
        {
            // ignored
        }
    }

    protected void BtnMinimize_OnClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    protected void BtnMaximize_OnClick(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
            _resizeImage.SetSource("Assets/fullscreen.png");
        }
        else
        {
            WindowState = WindowState.Maximized;
            _resizeImage.SetSource("Assets/fullscreen_exit.png");
        }
    }

    protected void BtnClose_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    protected void SetLanguageDictionary()
    {
        var dict = new ResourceDictionary();

        switch (Thread.CurrentThread.CurrentCulture.ToString())
        {
            case "en-US":
                dict.Source = new Uri("Resources\\StringResources.xaml", UriKind.Relative);
                break;
            case "es":
                dict.Source = new Uri("Resources\\StringResources-es.xaml", UriKind.Relative);
                break;
            default:
                dict.Source = new Uri("Resources\\StringResources.xaml", UriKind.Relative);
                break;
        }

        Resources.MergedDictionaries.Add(dict);
    }
}

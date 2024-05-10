using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Branta.Classes;

public class BaseWindow : Window
{
    private System.Windows.Controls.Image _resizeImage;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = "")
    {
        var handler = PropertyChanged;
        handler?.Invoke(this, new PropertyChangedEventArgs(name));
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
            BorderThickness = new Thickness(0);
            _resizeImage.SetSource("Assets/fullscreen.png");
        }
        else
        {
            WindowState = WindowState.Maximized;
            BorderThickness = new Thickness(8);
            _resizeImage.SetSource("Assets/fullscreen_exit.png");
        }
    }

    protected void BtnClose_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    protected ResourceDictionary SetLanguageDictionary()
    {
        var resourceDictionary = GetLanguageDictionary();

        Resources.MergedDictionaries.Add(resourceDictionary);

        return resourceDictionary;
    }

    public static ResourceDictionary GetLanguageDictionary()
    {
        var resourceDictionary = new ResourceDictionary();

        switch (Thread.CurrentThread.CurrentCulture.ToString())
        {
            case "en-US":
                resourceDictionary.Source = new Uri("Resources\\StringResources.xaml", UriKind.Relative);
                break;
            case "es":
            case "es-US":
                resourceDictionary.Source = new Uri("Resources\\StringResources-es.xaml", UriKind.Relative);
                break;
            default:
                resourceDictionary.Source = new Uri("Resources\\StringResources.xaml", UriKind.Relative);
                break;
        }

        return resourceDictionary;
    }
}
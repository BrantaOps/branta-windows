using System.Windows;
using System.Windows.Input;

namespace Branta.Classes;

public class BaseWindow : Window
{
    private System.Windows.Controls.Image _resizeImage;

    public void SetResizeImage(System.Windows.Controls.Image resizeImage)
    {
        _resizeImage = resizeImage;
    }

    protected void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
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
}

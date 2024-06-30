using System.Windows;
using System.Windows.Input;
using Color = Branta.Enums.Color;

namespace Branta.Classes;

public class BaseWindow : Window
{
    private System.Windows.Controls.Image _resizeImage;

    public BaseWindow()
    {
        Background = Color.Brush(Color.WindowBackground);
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
}
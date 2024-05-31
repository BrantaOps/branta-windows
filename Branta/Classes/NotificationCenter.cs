using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Branta.Commands;
using Branta.Stores;
using Application = System.Windows.Application;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace Branta.Classes;

public class NotificationCenter
{
    private readonly LanguageStore _languageStore;
    private readonly NotifyIcon _notifyIcon;

    public NotifyIcon NotifyIcon => _notifyIcon;

    private MainWindow _mainWindow;

    private HelpCommand HelpCommand { get; set; }
    private OpenSettingsWindowCommand OpenSettingsWindowCommand { get; set; }

    public NotificationCenter(LanguageStore languageStore)
    {
        _languageStore = languageStore;


        _notifyIcon = new NotifyIcon
        {
            Icon = new Icon(Application.GetResourceStream(new Uri("pack://application:,,,/Assets/black_circle.ico"))!.Stream),
            Text = "Branta",
            Visible = true
        };
    }

    public void Setup(MainWindow mainWindow, OpenSettingsWindowCommand openSettingsWindowCommand)
    {
        _mainWindow = mainWindow;

        HelpCommand = new HelpCommand();
        OpenSettingsWindowCommand = openSettingsWindowCommand;
        
        _notifyIcon.MouseClick += OnClick_NotifyIcon;
        _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
        _notifyIcon.ContextMenuStrip.Items.Add(_languageStore.Get("NotifyIcon_Settings"), null, OnClick_Settings);
        _notifyIcon.ContextMenuStrip.Items.Add(_languageStore.Get("NotifyIcon_Quit"), null, OnClick_Quit);
        _notifyIcon.ContextMenuStrip.Items.Add(_languageStore.Get("Help"), null, OnClick_Help);
    }

    public void Notify(Notification notification)
    {
        _notifyIcon.ShowBalloonTip(notification.Timeout, notification.Title, notification.Message ?? " ", notification.Icon);
    }
    
    private void OnClick_NotifyIcon(object sender, MouseEventArgs e)
    {
        switch (e.Button)
        {
            case MouseButtons.Left:
                _mainWindow.WindowState = WindowState.Normal;
                _mainWindow.Activate();
                _mainWindow.Show();
                break;
            case MouseButtons.Right:
                NotifyIcon.ContextMenuStrip?.Show();
                break;
        }
    }
    
    private void OnClick_Settings(object sender, EventArgs e)
    {
        OpenSettingsWindowCommand.Execute(null);
    }

    private void OnClick_Quit(object sender, EventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void OnClick_Help(object sender, EventArgs e)
    {
        HelpCommand.Execute(null);
    }
}

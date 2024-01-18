using Branta.Automation;
using Branta.Domain;
using Branta.Views;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace Branta;

public partial class MainWindow : Window
{
    private readonly NotifyIcon _notifyIcon;

    public VerifyWallets VerifyWallets { get; }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        _notifyIcon = new NotifyIcon
        {
            Icon = new System.Drawing.Icon("Assets/black_circle.ico"),
            Text = "Branta",
            Visible = true
        };
        _notifyIcon.DoubleClick += OnClick_NotifyIcon;
        _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
        _notifyIcon.ContextMenuStrip.Items.Add("Quit", null, OnClick_Quit);

        VerifyWallets = new VerifyWallets(_notifyIcon);

        VerifyWallets.Run();

        var timer = new System.Timers.Timer(VerifyWallets.RunInterval * 1000);
        timer.Elapsed += (_, _) => Dispatcher.Invoke(() => VerifyWallets.Run());
        timer.Start();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (Debugger.IsAttached) return;

        e.Cancel = true;
        Hide();
        base.OnClosing(e);
    }
    
    private void OnClick_NotifyIcon(object sender, EventArgs e)
    {
        WindowState = WindowState.Normal;
        Activate();
        Show();
    }

    private static void OnClick_Quit(object sender, EventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }

    private void OnClick_Help(object sender, MouseButtonEventArgs e)
    {
        var helpWindow = new HelpWindow
        {
            Topmost = true
        };

        helpWindow.Show();
    }

    public void OnClick_Status(object sender, MouseButtonEventArgs e)
    {
        var textBlock = (TextBlock) sender;
        var wallet = (Wallet) textBlock.Tag;

        var walletDetailWindow = new WalletDetailWindow(wallet)
        {
            Topmost = true
        };

        walletDetailWindow.Show();
    }
}
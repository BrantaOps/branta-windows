using Branta.Automation;
using Branta.Domain;
using Branta.Views;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Clipboard = System.Windows.Clipboard;

namespace Branta;

public partial class MainWindow : Window
{
    private readonly NotifyIcon _notifyIcon;
    private readonly System.Timers.Timer _verifyWalletTimer;
    private readonly System.Timers.Timer _clipboardGuardianTimer;

    public VerifyWallets VerifyWallets { get; }
    private ClipboardGuardian _clipboardGuardian;

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
        _verifyWalletTimer = VerifyWallets.CreateTimer();
        VerifyWallets.Elapsed(null, null);

        _clipboardGuardian = new ClipboardGuardian(_notifyIcon);
        _clipboardGuardianTimer = _clipboardGuardian.CreateTimer();
        _clipboardGuardian.Elapsed(null, null);

        var text = Clipboard.GetText();
        Trace.WriteLine($"Clipboard: {text}");
    }

    protected override void OnClosing(CancelEventArgs e)
    {
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

    private void OnClick_Quit(object sender, EventArgs e)
    {
        _verifyWalletTimer.Dispose();
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
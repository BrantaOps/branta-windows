using Branta.Automation;
using Branta.Classes;
using Branta.Domain;
using Branta.Views;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Application = System.Windows.Application;

namespace Branta;

public partial class MainWindow : BaseWindow
{
    private readonly NotifyIcon _notifyIcon;
    private readonly System.Timers.Timer _verifyWalletTimer;
    private readonly System.Timers.Timer _clipboardGuardianTimer;
    private readonly System.Timers.Timer _focusTimer;

    public VerifyWallets VerifyWallets { get; }

    public MainWindow()
    {
        try
        {
            InitializeComponent();
            DataContext = this;

            SetResizeImage(ImageScreenSize);

            _notifyIcon = new NotifyIcon
            {
                Icon = new Icon(Application.GetResourceStream(new Uri("pack://application:,,,/Assets/black_circle.ico"))!.Stream),
                Text = "Branta",
                Visible = true
            };
            _notifyIcon.DoubleClick += OnClick_NotifyIcon;
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Quit", null, OnClick_Quit);

            VerifyWallets = new VerifyWallets(_notifyIcon);
            _verifyWalletTimer = VerifyWallets.CreateTimer();
            VerifyWallets.Elapsed(null, null);

            var clipboardGuardian = new ClipboardGuardian(_notifyIcon);
            _clipboardGuardianTimer = clipboardGuardian.CreateTimer();
            clipboardGuardian.Elapsed(null, null);

            var focus = new Focus(_notifyIcon);
            _focusTimer = focus.CreateTimer();
            focus.Elapsed(null, null);
        }
        catch (Exception ex)
        {
            Trace.Listeners.Add(new TextWriterTraceListener("log.txt"));
            Trace.WriteLine(ex);
            Trace.Flush();
        }
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
        _clipboardGuardianTimer.Dispose();
        _focusTimer.Dispose();
        Application.Current.Shutdown();
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
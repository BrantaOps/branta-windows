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
using DataFormats = System.Windows.DataFormats;
using DragEventArgs = System.Windows.DragEventArgs;
using Timer = System.Timers.Timer;

namespace Branta;

public partial class MainWindow : BaseWindow
{
    private readonly NotifyIcon _notifyIcon;
    private readonly Installer _installer;
    private readonly Timer _clipboardGuardianTimer;
    private readonly Timer _focusTimer;
    private readonly Timer _updateTimer;
    private readonly Timer _installerTimer;
    private readonly Timer _loadCheckSumsTimer;

    private Timer _verifyWalletTimer;

    private Settings _settings;
    public event Action<Settings> SettingsChanged;

    public VerifyWallets VerifyWallets { get; }
    public LoadCheckSums LoadCheckSums { get; }

    public MainWindow()
    {
        try
        {
            InitializeComponent();
            DataContext = this;

            var resourceDictionary = SetLanguageDictionary();
            Analytics.Init();
            _settings = Settings.Load();
            SetResizeImage(ImageScreenSize);

            _notifyIcon = new NotifyIcon
            {
                Icon = new Icon(Application.GetResourceStream(new Uri("pack://application:,,,/Assets/black_circle.ico"))!.Stream),
                Text = "Branta",
                Visible = true
            };
            _notifyIcon.DoubleClick += OnClick_NotifyIcon;
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Settings", null, OnClick_Settings);
            _notifyIcon.ContextMenuStrip.Items.Add("Quit", null, OnClick_Quit);

            LoadCheckSums = new LoadCheckSums();
            _loadCheckSumsTimer = LoadCheckSums.CreateTimer();

            VerifyWallets = new VerifyWallets(_notifyIcon, _settings, LoadCheckSums);
            VerifyWallets.SubscribeToSettingsChanges(this);
            _verifyWalletTimer = VerifyWallets.CreateTimer();

            Task.Run(async () =>
            {
                await LoadCheckSums.LoadAsync();

                VerifyWallets.Elapsed(null, null);
            });

            var clipboardGuardian = new ClipboardGuardian(_notifyIcon, _settings);
            clipboardGuardian.SubscribeToSettingsChanges(this);
            _clipboardGuardianTimer = clipboardGuardian.CreateTimer();
            clipboardGuardian.Elapsed(null, null);

            var focus = new Focus(_notifyIcon, _settings);
            focus.SubscribeToSettingsChanges(this);
            _focusTimer = focus.CreateTimer();
            focus.Elapsed(null, null);

            var update = new UpdateApp(_notifyIcon, resourceDictionary);
            _updateTimer = update.CreateTimer();
            update.Elapsed(null, null);

            _installer = new Installer(_notifyIcon, resourceDictionary);
            _installerTimer = _installer.CreateTimer();
            _installer.Elapsed(null, null);
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
        _updateTimer.Dispose();
        _installerTimer.Dispose();
        _loadCheckSumsTimer.Dispose();
        Application.Current.Shutdown();
    }
    
    private void OnClick_Settings(object sender, EventArgs e)
    {
        var settingsWindow = new SettingsWindow(_settings);

        var result = settingsWindow.ShowDialog();

        var settings = settingsWindow.GetSettings();

        if (settings.WalletVerification.WalletVerifyEvery != _settings.WalletVerification.WalletVerifyEvery)
        {
            VerifyWallets.RunInterval = (int)settings.WalletVerification.WalletVerifyEvery.TotalSeconds;
            _verifyWalletTimer.Dispose();
            _verifyWalletTimer = VerifyWallets.CreateTimer();
        }

        Settings.Save(settings);

        _settings = settings;
        SettingsChanged?.Invoke(_settings);
    }

    private void OnClick_Help(object sender, MouseButtonEventArgs e)
    {
        var helpWindow = new HelpWindow();

        helpWindow.Show();
    }

    public void OnClick_Status(object sender, MouseButtonEventArgs e)
    {
        var textBlock = (TextBlock) sender;
        var wallet = (Wallet) textBlock.Tag;

        var walletDetailWindow = new WalletDetailWindow(wallet);

        walletDetailWindow.Show();
    }
    
    private void VerifyInstaller_Drop(object sender, DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            return;
        }

        var files = (string[])e.Data.GetData(DataFormats.FileDrop);

        _installer.ProcessFiles(files);
    }
}
using Branta.Automation;
using Branta.Classes;
using Branta.Domain;
using Branta.Views;
using CountlySDK;
using CountlySDK.Entities;
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
    private readonly ResourceDictionary _resourceDictionary;
    private readonly Installer _installer;
    private readonly Timer _clipboardGuardianTimer;
    private readonly Timer _focusTimer;
    private readonly Timer _updateTimer;
    private readonly Timer _installerTimer;

    private Timer _verifyWalletTimer;

    private Settings _settings;
    public event Action<Settings> SettingsChanged;

    public VerifyWallets VerifyWallets { get; }

    public MainWindow()
    {
        try
        {
            InitializeComponent();
            DataContext = this;

            _resourceDictionary = SetLanguageDictionary();
            InitCountly();
            LoadSettings();
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

            VerifyWallets = new VerifyWallets(_notifyIcon, _settings);
            VerifyWallets.SubscribeToSettingsChanges(this);
            _verifyWalletTimer = VerifyWallets.CreateTimer();
            VerifyWallets.Elapsed(null, null);

            var clipboardGuardian = new ClipboardGuardian(_notifyIcon, _settings);
            clipboardGuardian.SubscribeToSettingsChanges(this);
            _clipboardGuardianTimer = clipboardGuardian.CreateTimer();
            clipboardGuardian.Elapsed(null, null);

            var focus = new Focus(_notifyIcon, _settings);
            focus.SubscribeToSettingsChanges(this);
            _focusTimer = focus.CreateTimer();
            focus.Elapsed(null, null);

            var update = new UpdateApp(_notifyIcon, _resourceDictionary);
            _updateTimer = update.CreateTimer();
            update.Elapsed(null, null);

            _installer = new Installer(_notifyIcon, _resourceDictionary);
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

    public void ChangeSettings(Settings newSettings)
    {
        Properties.Settings.Default.BitcoinAddressesEnabled = newSettings.ClipboardGuardian.BitcoinAddressesEnabled;
        Properties.Settings.Default.SeedPhraseEnabled = newSettings.ClipboardGuardian.SeedPhraseEnabled;
        Properties.Settings.Default.ExtendedPublicKeyEnabled = newSettings.ClipboardGuardian.ExtendedPublicKeyEnabled;
        Properties.Settings.Default.PrivateKeyEnabled = newSettings.ClipboardGuardian.PrivateKeyEnabled;
        Properties.Settings.Default.NostrPublicKeyEnabled = newSettings.ClipboardGuardian.NostrPublicKeyEnabled;
        Properties.Settings.Default.NostrPrivateKeyEnabled = newSettings.ClipboardGuardian.NostrPrivateKeyEnabled;

        Properties.Settings.Default.WalletVerifyEvery = newSettings.WalletVerification.WalletVerifyEvery;
        Properties.Settings.Default.LaunchingWalletEnabled = newSettings.WalletVerification.LaunchingWalletEnabled;
        Properties.Settings.Default.WalletStatusChangeEnabled = newSettings.WalletVerification.WalletStatusChangeEnabled;

        Properties.Settings.Default.Save();

        _settings = newSettings;
        SettingsChanged?.Invoke(_settings);
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

        ChangeSettings(settings);
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

    private void LoadSettings()
    {
        _settings = new Settings
        {
            ClipboardGuardian = new ClipboardGuardianSettings
            {
                BitcoinAddressesEnabled = Properties.Settings.Default.BitcoinAddressesEnabled,
                SeedPhraseEnabled = Properties.Settings.Default.SeedPhraseEnabled,
                ExtendedPublicKeyEnabled = Properties.Settings.Default.ExtendedPublicKeyEnabled,
                PrivateKeyEnabled = Properties.Settings.Default.PrivateKeyEnabled,
                NostrPublicKeyEnabled = Properties.Settings.Default.NostrPublicKeyEnabled,
                NostrPrivateKeyEnabled = Properties.Settings.Default.NostrPrivateKeyEnabled
            },
            WalletVerification = new WalletVerificationSettings
            {
                WalletVerifyEvery = Properties.Settings.Default.WalletVerifyEvery,
                LaunchingWalletEnabled = Properties.Settings.Default.LaunchingWalletEnabled,
                WalletStatusChangeEnabled = Properties.Settings.Default.WalletStatusChangeEnabled
            }
        };
    }

    private static void InitCountly()
    {
        var cc = new CountlyConfig
        {
            serverUrl = "https://branta-0dc12e4ffb389.flex.countly.com",
            appKey = "ccc4eb59a850e5f3bdf640b8d36284c3bce03f12",
            appVersion = Helper.GetBrantaVersionWithoutCommitHash()
        };

        Countly.Instance.Init(cc);
        Countly.Instance.SessionBegin();
    }
}
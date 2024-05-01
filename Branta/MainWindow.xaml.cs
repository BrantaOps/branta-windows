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
    private readonly Focus _focus;
    private readonly Timer _clipboardGuardianTimer;
    private readonly Timer _focusTimer;
    private readonly Timer _updateTimer;
    private readonly Timer _installerTimer;
    private readonly Timer _loadCheckSumsTimer;

    private Timer _verifyWalletTimer;

    private Settings _settings;
    private bool _isLoading = true;
    public event Action<Settings> SettingsChanged;

    public VerifyWallets VerifyWallets { get; }
    public LoadCheckSums LoadCheckSums { get; }

    public MainWindow()
    {
        try
        {
            InitializeComponent();
            DataContext = this;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

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

            var clipboardGuardian = new ClipboardGuardian(_notifyIcon, _settings);
            clipboardGuardian.SubscribeToSettingsChanges(this);
            _clipboardGuardianTimer = clipboardGuardian.CreateTimer();
            clipboardGuardian.Elapsed(null, null);

            _focus = new Focus(_notifyIcon, _settings);
            _focus.SubscribeToSettingsChanges(this);
            _focusTimer = _focus.CreateTimer();

            var update = new UpdateApp(_notifyIcon, resourceDictionary);
            _updateTimer = update.CreateTimer();
            update.Elapsed(null, null);

            _installer = new Installer(_notifyIcon, resourceDictionary);
            _installerTimer = _installer.CreateTimer();

            Task.Run(async () =>
            {
                var checkSumsTask = Task.Run(LoadCheckSums.LoadAsync);
                var installerHashTask = Task.Run(_installer.LoadAsync);

                await Task.WhenAll(checkSumsTask, installerHashTask);

                Dispatcher.Invoke(OnLoadComplete);
            });

            var args = Environment.GetCommandLineArgs();

            if (args.Contains("headless"))
            {
                OnClosing(new CancelEventArgs());
            }
        }
        catch (Exception ex)
        {
            Trace.Listeners.Add(new TextWriterTraceListener("log.txt"));
            Trace.WriteLine(ex);
            Trace.Flush();
        }
    }

    private void OnLoadComplete()
    {
        _isLoading = false;

        TbCheckSumsLoading.Visibility = Visibility.Hidden;
        TbWalletsDetected.Visibility = Visibility.Visible;

        VerifyWallets.Elapsed(null, null);

        _focus.Elapsed(null, null);
        _focus.SetWallets(LoadCheckSums.WalletTypes);
    }

    protected sealed override void OnClosing(CancelEventArgs e)
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
            VerifyWallets.RunInterval = TimeSpan.FromSeconds(settings.WalletVerification.WalletVerifyEvery.TotalSeconds);
            _verifyWalletTimer.Dispose();
            _verifyWalletTimer = VerifyWallets.CreateTimer();
        }

        Settings.Save(settings);

        _settings = settings;
        SettingsChanged?.Invoke(_settings);
    }

    private void OnClick_Help(object sender, MouseButtonEventArgs e)
    {
        var helpWindow = new HelpWindow(this)
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        helpWindow.Show();
    }

    public void OnClick_Status(object sender, MouseButtonEventArgs e)
    {
        var textBlock = (TextBlock) sender;
        var wallet = (Wallet) textBlock.Tag;

        var walletDetailWindow = new WalletDetailWindow(this, wallet)
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        walletDetailWindow.Show();
    }

    private void OnClick_NetworkActivityDetails(object sender, MouseButtonEventArgs e)
    {
        var textBlock = (TextBlock) sender;
        var wallet = (Wallet) textBlock.Tag;

        var networkActivityWindow = new NetworkActivityWindow(this, wallet)
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        networkActivityWindow.Show();
    }

    private void OnClick_BrowseFiles(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Multiselect = true
        };

        openFileDialog.ShowDialog();

        var files = openFileDialog.FileNames;

        _installer.ProcessFiles(files);
    }

    private void VerifyInstaller_Drop(object sender, DragEventArgs e)
    {
        if (_isLoading)
        {
            _notifyIcon.ShowBalloonTip(new Notification
            {
                Message = "Installer hashes are still loading.",
                Icon = ToolTipIcon.Info
            });
            return;
        }

        if (!e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            return;
        }

        var files = (string[])e.Data.GetData(DataFormats.FileDrop);

        _installer.ProcessFiles(files);
    }
}
using Branta.Classes;
using Branta.Commands;
using Branta.Stores;
using Branta.ViewModels;
using Branta.Windows;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Application = System.Windows.Application;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace Branta;

public partial class MainWindow
{
    private readonly Settings _settings;
    private readonly WalletVerificationViewModel _walletVerificationViewModel;
    private readonly InstallerVerificationViewModel _installerVerificationViewModel;
    private readonly CheckSumStore _checkSumStore;
    private readonly NotificationCenter _notificationCenter;
    private readonly InstallerHashStore _installerHashStore;

    private ICommand HelpCommand { get; }

    public MainWindow(NotificationCenter notificationCenter, Settings settings, ResourceDictionary resourceDictionary,
        WalletVerificationViewModel walletVerificationViewModel, CheckSumStore checkSumStore, InstallerHashStore installerHashStore, InstallerVerificationViewModel installerVerificationViewModel)
    {
        HelpCommand = new HelpCommand();

        _settings = settings;
        _walletVerificationViewModel = walletVerificationViewModel;
        _installerVerificationViewModel = installerVerificationViewModel;
        _checkSumStore = checkSumStore;
        _installerHashStore = installerHashStore;

        MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;

        _notificationCenter = notificationCenter;

        _notificationCenter.NotifyIcon.MouseClick += OnClick_NotifyIcon;
        _notificationCenter.NotifyIcon.ContextMenuStrip = new ContextMenuStrip();
        _notificationCenter.NotifyIcon.ContextMenuStrip.Items.Add(resourceDictionary["NotifyIcon_Settings"].ToString(), null, OnClick_Settings);
        _notificationCenter.NotifyIcon.ContextMenuStrip.Items.Add(resourceDictionary["NotifyIcon_Quit"].ToString(), null, OnClick_Quit);
        _notificationCenter.NotifyIcon.ContextMenuStrip.Items.Add(resourceDictionary["Help"].ToString(), null, OnClick_Help);

        try
        {
            InitializeComponent();

            SetLanguageDictionary();
            Analytics.Init();
            SetResizeImage(ImageScreenSize);

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

    protected sealed override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
        base.OnClosing(e);
    }


    private void OnClick_NotifyIcon(object sender, MouseEventArgs e)
    {
        switch (e.Button)
        {
            case MouseButtons.Left:
                WindowState = WindowState.Normal;
                Activate();
                Show();
                break;
            case MouseButtons.Right:
                _notificationCenter.NotifyIcon.ContextMenuStrip?.Show();
                break;
        }
    }

    private void OnClick_Quit(object sender, EventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void OnClick_Help(object sender, EventArgs e)
    {
        HelpCommand.Execute(null);
    }

    private void OnClick_Settings(object sender, EventArgs e)
    {
        var settingsWindow = new SettingsWindow(_settings, _checkSumStore, _installerHashStore, _walletVerificationViewModel, _installerVerificationViewModel);

        settingsWindow.ShowDialog();

        var settings = settingsWindow.GetSettings();

        if (settings.WalletVerification.WalletVerifyEvery != _settings.WalletVerification.WalletVerifyEvery)
        {
            _walletVerificationViewModel.SetTimer(settings.WalletVerification.WalletVerifyEvery);
        }

        _settings.Update(settings);
    }
}
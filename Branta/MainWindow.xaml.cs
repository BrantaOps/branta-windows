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

namespace Branta;

public partial class MainWindow
{
    private readonly Settings _settings;
    private readonly WalletVerificationViewModel _walletVerificationViewModel;
    private readonly CheckSumStore _checkSumStore;
    private ICommand HelpCommand { get; }

    public MainWindow(NotificationCenter notificationCenter, Settings settings, ResourceDictionary resourceDictionary,
        WalletVerificationViewModel walletVerificationViewModel, CheckSumStore checkSumStore)
    {
        HelpCommand = new HelpCommand();

        _settings = settings;
        _walletVerificationViewModel = walletVerificationViewModel;
        _checkSumStore = checkSumStore;

        MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;

        notificationCenter.NotifyIcon.Click += OnClick_NotifyIcon;
        notificationCenter.NotifyIcon.ContextMenuStrip = new ContextMenuStrip();
        notificationCenter.NotifyIcon.ContextMenuStrip.Items.Add(resourceDictionary["NotifyIcon_Settings"].ToString(), null, OnClick_Settings);
        notificationCenter.NotifyIcon.ContextMenuStrip.Items.Add(resourceDictionary["NotifyIcon_Quit"].ToString(), null, OnClick_Quit);
        notificationCenter.NotifyIcon.ContextMenuStrip.Items.Add(resourceDictionary["Help"].ToString(), null, OnClick_Help);

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


    private void OnClick_NotifyIcon(object sender, EventArgs e)
    {
        WindowState = WindowState.Normal;
        Activate();
        Show();
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
        var settingsWindow = new SettingsWindow(_settings, _checkSumStore, _walletVerificationViewModel);

        settingsWindow.ShowDialog();

        var settings = settingsWindow.GetSettings();

        if (settings.WalletVerification.WalletVerifyEvery != _settings.WalletVerification.WalletVerifyEvery)
        {
            _walletVerificationViewModel.SetTimer(settings.WalletVerification.WalletVerifyEvery);
        }

        _settings.Update(settings);
    }
}
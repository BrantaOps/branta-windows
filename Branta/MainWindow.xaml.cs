using Branta.Classes;
using Branta.Commands;
using Branta.Stores;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Application = System.Windows.Application;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace Branta;

public partial class MainWindow
{
    private readonly NotificationCenter _notificationCenter;

    private ICommand HelpCommand { get; }

    private ICommand OpenSettingsWindowCommand { get; }

    public MainWindow(NotificationCenter notificationCenter, LanguageStore languageStore, AppSettings appSettings,
        OpenSettingsWindowCommand openSettingsWindowCommand, ILogger<MainWindow> logger)
    {
        HelpCommand = new HelpCommand();
        OpenSettingsWindowCommand = openSettingsWindowCommand;

        MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;

        _notificationCenter = notificationCenter;

        _notificationCenter.NotifyIcon.MouseClick += OnClick_NotifyIcon;
        _notificationCenter.NotifyIcon.ContextMenuStrip = new ContextMenuStrip();
        _notificationCenter.NotifyIcon.ContextMenuStrip.Items.Add(languageStore.Get("NotifyIcon_Settings"), null, OnClick_Settings);
        _notificationCenter.NotifyIcon.ContextMenuStrip.Items.Add(languageStore.Get("NotifyIcon_Quit"), null, OnClick_Quit);
        _notificationCenter.NotifyIcon.ContextMenuStrip.Items.Add(languageStore.Get("Help"), null, OnClick_Help);

        try
        {
            InitializeComponent();

            TbVersion.Text = Helper.GetBrantaVersionWithoutCommitHash();

            SetLanguageDictionary(languageStore);
            Analytics.Init(appSettings);
            SetResizeImage(ImageScreenSize);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            System.Windows.MessageBox.Show("Branta Failed to Start.", "Branta Exception");
            Application.Current.Shutdown();
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
        OpenSettingsWindowCommand.Execute(null);
    }
}
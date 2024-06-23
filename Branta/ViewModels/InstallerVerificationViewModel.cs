using Branta.Classes;
using Branta.Commands;
using Branta.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace Branta.ViewModels;

public partial class InstallerVerificationViewModel : ObservableObject
{
    private readonly NotificationCenter _notificationCenter;
    private readonly LanguageStore _languageStore;
    private readonly Timer _timer;

    private Dictionary<string, string> _installerHashes;

	[ObservableProperty]
    private bool _isLoading = true;

    public LoadInstallerHashesCommand LoadInstallerHashesCommand { get; }

    [RelayCommand]
    public void BrowseFiles()
    {
        var openFileDialog = new OpenFileDialog
        {
            Multiselect = true
        };

        openFileDialog.ShowDialog();

        var files = openFileDialog.FileNames.ToList();

        ProcessFiles(files);
    }

    [RelayCommand]
    public void DropFiles(object parameter)
    {
        var dragEventArgs = (System.Windows.DragEventArgs) parameter;

        if (!dragEventArgs.Data.GetDataPresent(DataFormats.FileDrop))
        {
            return;
        }

        var files = (string[])dragEventArgs.Data.GetData(DataFormats.FileDrop);

        if (files == null)
        {
            return;
        }

        ProcessFiles(files.ToList());
    }

    public InstallerVerificationViewModel(NotificationCenter notificationCenter, LanguageStore languageStore, LoadInstallerHashesCommand loadInstallerHashesCommand)
    {
        _notificationCenter = notificationCenter;
        _languageStore = languageStore;

        LoadInstallerHashesCommand = loadInstallerHashesCommand;
        LoadInstallerHashesCommand.Execute(this);

        _timer = new Timer(new TimeSpan(0, 30, 0));
        _timer.Elapsed += (_, _) => LoadInstallerHashesCommand.Execute(this);
        _timer.Start();
    }

    public void ProcessFiles(List<string> files)
    {
        if (IsLoading)
        {
            _notificationCenter.Notify(new Notification
            {
                Message = _languageStore.Get("InstallerHashesLoading"),
                Icon = ToolTipIcon.Info
            });
            return;
        }

        foreach (var file in files)
        {
            var filename = _installerHashes.GetValueOrDefault(Helper.CalculateSha256(file)) ??
                           _installerHashes.GetValueOrDefault(Helper.CalculateSha512(file)) ??
                           _installerHashes.GetValueOrDefault(Helper.CalculateSha512(file, base64Encoding: true));

            _notificationCenter.Notify(filename != null
                ? new Notification
                {
                    Message = _languageStore.Get("InstallerValid"),
                    Icon = ToolTipIcon.None
                }
                : new Notification
                {
                    Message = _languageStore.Get("InstallerInvalid"),
                    Icon = ToolTipIcon.Error
                });
        }
    }

    public void SetInstallerHashes(Dictionary<string, string> installerHashes)
    {
        _installerHashes = installerHashes;
    }
}
using Branta.Classes;
using Branta.Commands;
using Branta.Stores;
using System.Windows.Forms;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace Branta.ViewModels;

public class InstallerVerificationViewModel : BaseViewModel
{
    private readonly NotificationCenter _notificationCenter;
    private readonly LanguageStore _languageStore;
    private readonly Timer _timer;

    private Dictionary<string, string> _installerHashes;

    private bool _isLoading = true;

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public LoadInstallerHashesCommand LoadInstallerHashesCommand { get; }
    public ICommand BrowseFilesCommand { get; }
    public ICommand DropFilesCommand { get; }

    public InstallerVerificationViewModel(NotificationCenter notificationCenter, LanguageStore languageStore, LoadInstallerHashesCommand loadInstallerHashesCommand)
    {
        _notificationCenter = notificationCenter;
        _languageStore = languageStore;

        LoadInstallerHashesCommand = loadInstallerHashesCommand;
        LoadInstallerHashesCommand.Execute(this);

        _timer = new Timer(new TimeSpan(0, 30, 0));
        _timer.Elapsed += (_, _) => LoadInstallerHashesCommand.Execute(this);
        _timer.Start();

        BrowseFilesCommand = new BrowseFilesCommand(ProcessFiles);
        DropFilesCommand = new DropFilesCommand(ProcessFiles);
    }

    public void ProcessFiles(List<string> files)
    {
        if (_isLoading)
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
using Branta.Classes;
using Branta.Commands;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace Branta.ViewModels;

public class InstallerVerificationViewModel : BaseViewModel
{
    private readonly NotificationCenter _notificationCenter;
    private readonly ResourceDictionary _resourceDictionary;
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

    public InstallerVerificationViewModel(NotificationCenter notificationCenter, ResourceDictionary resourceDictionary)
    {
        _notificationCenter = notificationCenter;
        _resourceDictionary = resourceDictionary;

        _timer = new Timer(new TimeSpan(0, 30, 0));
        _timer.Elapsed += (object sender, ElapsedEventArgs e) => LoadInstallerHashesCommand.Execute(null);
        _timer.Start();

        LoadInstallerHashesCommand = new LoadInstallerHashesCommand(this);
        BrowseFilesCommand = new BrowseFilesCommand(ProcessFiles);
        DropFilesCommand = new DropFilesCommand(ProcessFiles);

        LoadInstallerHashesCommand.Execute(null);
    }

    public void ProcessFiles(List<string> files)
    {
        if (_isLoading)
        {
            _notificationCenter.Notify(new Notification
            {
                Message = "Installer hashes are still loading.",
                Icon = ToolTipIcon.Info
            });
            return;
        }

        foreach (var file in files)
        {
            var filename = _installerHashes.GetValueOrDefault(Helper.CalculateSha256(file)) ??
                           _installerHashes.GetValueOrDefault(Helper.CalculateSha512(file)) ??
                           _installerHashes.GetValueOrDefault(Helper.CalculateSha512(file, base64Encoding: true));

            _notificationCenter.Notify(filename != null ? new Notification()
            {
                Message = (string)_resourceDictionary["InstallerValid"],
                Icon = ToolTipIcon.None
            } : new Notification()
            {
                Message = (string)_resourceDictionary["InstallerInvalid"],
                Icon = ToolTipIcon.Error
            });
        }
    }

    public void SetInstallerHashes(Dictionary<string, string> installerHashes)
    {
        _installerHashes = installerHashes;
    }
}

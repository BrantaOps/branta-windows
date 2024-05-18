using Branta.Classes;
using System.Windows;
using System.Windows.Forms;

namespace Branta.Commands;

public class UpdateAppCommand : BaseAsyncCommand
{
    private readonly NotificationCenter _notificationCenter;
    private readonly ResourceDictionary _resourceDictionary;
    private readonly GitHubClient _gitHubClient;

    private bool _isUpdateAvailableShow;

    public UpdateAppCommand(NotificationCenter notificationCenter, ResourceDictionary resourceDictionary)
    {
        _notificationCenter = notificationCenter;
        _resourceDictionary = resourceDictionary;
        _gitHubClient = new GitHubClient();
    }

    public override async Task ExecuteAsync(object parameter)
    {
        var latestVersion = await _gitHubClient.GetLatestReleaseVersionAsync();

        if (latestVersion == null)
        {
            return;
        }

        var installedVersion = Helper.GetBrantaVersionWithoutCommitHash();

        if (latestVersion != installedVersion)
        {
            _notificationCenter.Notify(new Notification
            {
                Icon = ToolTipIcon.Info,
                Message = string.Format(_resourceDictionary["UpdateApp"]?.ToString(), latestVersion)
            });

            if (_isUpdateAvailableShow == false)
            {
                _notificationCenter.NotifyIcon.ContextMenuStrip?
                    .Items.Add(_resourceDictionary["NotifyIcon_UpdateAvailable"]?.ToString(), null, OnClick_UpdateAvailable);
                _isUpdateAvailableShow = true;
            }
        }
    }

    private static void OnClick_UpdateAvailable(object sender, EventArgs e)
    {
        Helper.OpenLink("https://branta.pro/download");
    }
}

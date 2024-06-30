using Branta.Classes;
using Branta.Features.Main;
using System.Windows.Forms;

namespace Branta.Commands;

public class UpdateAppCommand : BaseAsyncCommand
{
    private readonly NotificationCenter _notificationCenter;
    private readonly LanguageStore _languageStore;
    private readonly GitHubClient _gitHubClient;

    private bool _isUpdateAvailableShow;

    public UpdateAppCommand(NotificationCenter notificationCenter, LanguageStore languageStore)
    {
        _notificationCenter = notificationCenter;
        _languageStore = languageStore;
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

        if (new Version(latestVersion) > new Version(installedVersion))
        {
            _notificationCenter.Notify(new Notification
            {
                Icon = ToolTipIcon.Info,
                Message = _languageStore.Format("UpdateApp", latestVersion)
            });

            if (_isUpdateAvailableShow == false)
            {
                _notificationCenter.NotifyIcon.ContextMenuStrip?.Items.Add(
                    _languageStore.Get("NotifyIcon_UpdateAvailable"), null, OnClick_UpdateAvailable);
                _isUpdateAvailableShow = true;
            }
        }
    }

    private static void OnClick_UpdateAvailable(object sender, EventArgs e)
    {
        Helper.OpenLink("https://branta.pro/download");
    }
}
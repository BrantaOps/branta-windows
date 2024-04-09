using Branta.Classes;
using System.Windows;
using System.Windows.Forms;

namespace Branta.Automation;

public class UpdateApp : BaseAutomation
{
    private readonly GitHubClient _githubClient;
    private readonly ResourceDictionary _resourceDictionary;
    private bool _isUpdateAvailableShow;

    public UpdateApp(NotifyIcon notifyIcon, ResourceDictionary resourceDictionary) : base(notifyIcon, null, 60 * 60 * 24)
    {
        _resourceDictionary = resourceDictionary;
        _githubClient = new GitHubClient();
    }

    public UpdateApp(NotifyIcon notifyIcon, Settings settings, int runInterval) : base(notifyIcon, settings,
        runInterval)
    {
    }

    public override void Run()
    {
        Task.Run(async () =>
        {
            var latestVersion = await _githubClient.GetLatestReleaseVersionAsync();

            if (latestVersion == null)
            {
                return;
            }

            var installedVersion = Helper.GetBrantaVersionWithoutCommitHash();

            if (latestVersion != installedVersion)
            {
                NotifyIcon.ShowBalloonTip(new Notification
                {
                    Icon = ToolTipIcon.Info,
                    Message = string.Format((string)_resourceDictionary["UpdateApp"] ?? string.Empty, latestVersion)
                });

                if (_isUpdateAvailableShow == false)
                {
                    NotifyIcon.ContextMenuStrip?.Items.Add("Update Available", null, OnClick_UpdateAvailable);
                    _isUpdateAvailableShow = true;
                }
            }
        });
    }

    private void OnClick_UpdateAvailable(object sender, EventArgs e)
    {
        Helper.OpenLink("https://branta.pro/download");
    }
}
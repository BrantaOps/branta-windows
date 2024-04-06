using Branta.Classes;
using System.Windows;
using System.Windows.Forms;

namespace Branta.Automation;

public class UpdateApp
{
    public static async Task CheckForNewReleaseAsync(NotifyIcon notifyIcon, ResourceDictionary resourceDictionary)
    {
        var client = new GitHubClient();

        var latestVersion = await client.GetLatestReleaseVersionAsync();
        var installedVersion = Helper.GetBrantaVersionWithoutCommitHash();

        if (latestVersion != installedVersion)
        {
            notifyIcon.ShowBalloonTip(new Notification
            {
                Icon = ToolTipIcon.Info,
                Message = string.Format((string)resourceDictionary["UpdateApp"], latestVersion)
            });
        }
    }
}
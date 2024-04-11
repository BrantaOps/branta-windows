using Branta.Classes;
using System.Windows;
using System.Windows.Forms;

namespace Branta.Automation;

public class Installer
{
    private readonly NotifyIcon _notifyIcon;
    private readonly ResourceDictionary _resourceDictionary;

    public Installer(NotifyIcon notifyIcon, ResourceDictionary resourceDictionary)
    {
        _notifyIcon = notifyIcon;
        _resourceDictionary = resourceDictionary;
    }

    public void ProcessFiles(string[] files)
    {
        var hashes = YamlLoader.LoadInstallerHashes();

        foreach (var file in files)
        {
            var filename = hashes.GetValueOrDefault(Helper.CalculateSha256(file)) ??
                           hashes.GetValueOrDefault(Helper.CalculateSha512(file)) ??
                           hashes.GetValueOrDefault(Helper.CalculateSha512(file, base64Encoding: true));

            _notifyIcon.ShowBalloonTip(filename != null ? new Notification
            {
                Message = (string)_resourceDictionary["InstallerValid"],
                Icon = ToolTipIcon.None
            } : new Notification
            {
                Message = (string)_resourceDictionary["InstallerInvalid"],
                Icon = ToolTipIcon.Error
            });
        }
    }
}
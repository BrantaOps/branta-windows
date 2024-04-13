using Branta.Classes;
using System.Windows;
using System.Windows.Forms;

namespace Branta.Automation;

public class Installer : BaseAutomation
{
    private readonly NotifyIcon _notifyIcon;
    private readonly ResourceDictionary _resourceDictionary;
    private readonly BrantaClient _brantaClient;

    private Dictionary<string, string> _hashes;

    public Installer(NotifyIcon notifyIcon, ResourceDictionary resourceDictionary) : base(notifyIcon, null, 60 * 60 * 4)
    {
        _notifyIcon = notifyIcon;
        _resourceDictionary = resourceDictionary;

        _brantaClient = new BrantaClient();
    }

    public void ProcessFiles(string[] files)
    {
        foreach (var file in files)
        {
            var filename = _hashes.GetValueOrDefault(Helper.CalculateSha256(file)) ??
                           _hashes.GetValueOrDefault(Helper.CalculateSha512(file)) ??
                           _hashes.GetValueOrDefault(Helper.CalculateSha512(file, base64Encoding: true));

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

    public override void Run()
    {
        Task.Run(async () => _hashes = await _brantaClient.GetInstallerHashesAsync() ?? YamlLoader.LoadInstallerHashes());
    }
}
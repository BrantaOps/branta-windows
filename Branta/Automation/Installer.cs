using Branta.Classes;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Forms;

namespace Branta.Automation;

public class Installer : BaseAutomation
{
    private readonly NotifyIcon _notifyIcon;
    private readonly ResourceDictionary _resourceDictionary;
    private readonly HttpClient _brantaHashClient;

    private Dictionary<string, string> _hashes;

    public Installer(NotifyIcon notifyIcon, ResourceDictionary resourceDictionary) : base(notifyIcon, null, 60)
    {
        _notifyIcon = notifyIcon;
        _resourceDictionary = resourceDictionary;

        _brantaHashClient = new HttpClient()
        {
            BaseAddress = new Uri("https://hash-server-7be6da1b0395.herokuapp.com")
        };
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
        Task.Run(LoadInstallerHashesAsync);
    }

    private async Task LoadInstallerHashesAsync()
    {
        _hashes = await FetchInstallerHashes() ?? YamlLoader.LoadInstallerHashes();
    }

    private async Task<Dictionary<string, string>> FetchInstallerHashes()
    {
        try
        {
            var response = await _brantaHashClient.GetAsync("/installer_hashes");
            var content = await response.Content.ReadAsStreamAsync();

            return YamlLoader.LoadInstallerHashes(new StreamReader(content));
        }
        catch
        {
            return null;
        }
    }
}
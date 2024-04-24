﻿using Branta.Classes;
using System.Windows;
using System.Windows.Forms;

namespace Branta.Automation;

public class Installer : BaseAutomation
{
    private readonly NotifyIcon _notifyIcon;
    private readonly ResourceDictionary _resourceDictionary;
    private readonly BrantaClient _brantaClient;

    private const string InstallerHashPath = "InstallerHash.yaml";

    private Dictionary<string, string> _hashes;

    public Installer(NotifyIcon notifyIcon, ResourceDictionary resourceDictionary) : base(notifyIcon, null,new TimeSpan(0, 30, 0))
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
        Task.Run(LoadAsync);
    }

    public async Task LoadAsync()
    {
        _hashes = await LoadInstallerHashesAsync();
    }

    public async Task<Dictionary<string, string>> LoadInstallerHashesAsync()
    {
        var serverHashes = await _brantaClient.GetInstallerHashesAsync();

        if (serverHashes != null)
        {
            FileStorage.Save(InstallerHashPath, serverHashes);

            return YamlLoader.Load<Dictionary<string, string>>(serverHashes);
        }

        var cacheHashes = FileStorage.Load(InstallerHashPath);

        if (cacheHashes != null)
        {
            return YamlLoader.Load<Dictionary<string, string>>(cacheHashes);
        }

        return YamlLoader.LoadInstallerHashes();
    }
}
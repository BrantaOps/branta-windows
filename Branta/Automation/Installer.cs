using Branta.Automation.Wallets;
using Branta.Classes;
using System.IO;
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
        foreach (var file in files)
        {
            foreach (var walletType in VerifyWallets.WalletTypes)
            {
                if (IsFileValid(file, walletType, out var notification))
                {
                    _notifyIcon.ShowBalloonTip(notification);
                    break;
                }
            }
        }
    }

    private  bool IsFileValid(string file, BaseWallet wallet, out Notification notification)
    {
        notification = null;
        var fileName = Path.GetFileName(file);

        if (!file.Contains(wallet.Name))
            return false;

        if (wallet.InstallerHashes.TryGetValue(fileName, out var expectedHash))
        {
            if (expectedHash == Helper.CalculateSha256(file))
            {
                notification = new Notification
                {
                    Message = (string)_resourceDictionary["InstallerValid"],
                    Icon = ToolTipIcon.None
                };
            }
            else
            {
                notification = new Notification
                {
                    Message = (string)_resourceDictionary["InstallerInvalid"],
                    Icon = ToolTipIcon.Error
                };
            }
        }
        else
        {
            notification = new Notification
            {
                Message = (string)_resourceDictionary["InstallerNotSupported"],
                Icon = ToolTipIcon.Warning
            };
        }

        return true;
    }
}
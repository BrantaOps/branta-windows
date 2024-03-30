using Branta.Automation.Wallets;
using Branta.Classes;
using Branta.Enums;
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
            var walletFound = false;
            foreach (var walletType in VerifyWallets.WalletTypes)
            {
                if (IsFileValid(file, walletType, out var notification))
                {
                    walletFound = true;
                    _notifyIcon.ShowBalloonTip(notification);
                    break;
                }
            }

            if (!walletFound)
            {
                _notifyIcon.ShowBalloonTip(new Notification
                {
                    Icon = ToolTipIcon.Warning,
                    Message = (string)_resourceDictionary["InstallerNotSupported"]
                });
            }
        }
    }

    private  bool IsFileValid(string file, BaseWallet wallet, out Notification notification)
    {
        notification = null;
        var fileName = Path.GetFileName(file);

        if (!file.Contains(wallet.InstallerName))
            return false;

        if (wallet.InstallerHashes.TryGetValue(fileName, out var expectedHash))
        {
            var actualHash = wallet.InstallerHashType switch
            {
                HashType.Sha256 =>  Helper.CalculateSha256(file),
                HashType.Sha512 =>  Helper.CalculateSha512(file),
                HashType.Sha512WithBase64Encode => Helper.CalculateSha512(file, base64Encoding: true),
                _ => throw new NotImplementedException()
            };

            if (expectedHash == actualHash)
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
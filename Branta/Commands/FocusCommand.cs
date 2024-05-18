using Branta.Classes;
using Branta.Classes.Wallets;
using Branta.Enums;
using Branta.Models;
using System.Diagnostics;
using System.Windows;

namespace Branta.Commands;

public class FocusCommand : BaseCommand
{
    private readonly NotificationCenter _notificationCenter;
    private readonly Settings _settings;
    private readonly ResourceDictionary _resourceDictionary;

    private Dictionary<BaseWallet, WalletStatus> _walletTypes;

    public FocusCommand(NotificationCenter notificationCenter, Settings settings, ResourceDictionary resourceDictionary)
    {
        _notificationCenter = notificationCenter;
        _settings = settings;
        _resourceDictionary = resourceDictionary;
    }

    public override void Execute(object parameter)
    {
        if (!_settings.WalletVerification.LaunchingWalletEnabled || _walletTypes == null)
        {
            return;
        }

        var processNames = Process.GetProcesses()
            .Select(p => p.ProcessName)
            .Distinct()
            .ToList();

        foreach (var walletType in _walletTypes.Keys)
        {
            if (!processNames.Contains(walletType.ExeName))
            {
                _walletTypes[walletType] = null;
                continue;
            }

            var (version, walletStatus) = VerifyWalletsCommand.Verify(walletType);

            var wallet = new Wallet()
            {
                Name = walletType.Name,
                Version = version,
                Status = walletStatus
            };

            if (wallet.Status != _walletTypes.GetValueOrDefault(walletType))
            {
                _notificationCenter.Notify(new Notification
                {
                    Message = $"{wallet.Name} {wallet.Version} is running. Status: {wallet.Status.GetName(_resourceDictionary)}"
                });

                _walletTypes[walletType] = wallet.Status;
            }
        }
    }

    public void SetWallets(List<BaseWallet> wallets)
    {
        _walletTypes = wallets.ToDictionary(w => w, _ => (WalletStatus)null);
    }
}

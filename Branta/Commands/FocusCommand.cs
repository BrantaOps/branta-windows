using Branta.Classes;
using Branta.Classes.Wallets;
using Branta.Enums;
using Branta.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Windows;

namespace Branta.Commands;

public class FocusCommand : BaseCommand
{
    private readonly NotificationCenter _notificationCenter;
    private readonly Settings _settings;
    private readonly ResourceDictionary _resourceDictionary;

    private Dictionary<BaseWalletType, WalletStatus> _walletTypes;
    private readonly ILogger<FocusCommand> _logger;

    public FocusCommand(NotificationCenter notificationCenter, Settings settings, ResourceDictionary resourceDictionary,
        ILogger<FocusCommand> logger)
    {
        _notificationCenter = notificationCenter;
        _settings = settings;
        _resourceDictionary = resourceDictionary;
        _logger = logger;
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

            var (version, walletStatus) = VerifyWalletsCommand.Verify(walletType, _logger);

            if (walletStatus == _walletTypes.GetValueOrDefault(walletType))
            {
                continue;
            }

            var wallet = new Wallet
            {
                Name = walletType.Name,
                Version = version,
                Status = walletStatus,
                ExeName = walletType.ExeName,
            };

            _notificationCenter.Notify(new Notification
            {
                Message = string.Format(_resourceDictionary["FocusMessage"]?.ToString() ?? "", wallet.Name,
                    wallet.Version, wallet.Status.GetName(_resourceDictionary))
            });

            _walletTypes[walletType] = wallet.Status;
        }
    }

    public void SetWallets(List<BaseWalletType> wallets)
    {
        _walletTypes = wallets.ToDictionary(w => w, _ => (WalletStatus)null);
    }
}
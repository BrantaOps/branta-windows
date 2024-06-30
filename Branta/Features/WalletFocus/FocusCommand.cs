using Branta.Classes;
using Branta.Classes.Wallets;
using Branta.Commands;
using Branta.Enums;
using Branta.Features.Main;
using Branta.Features.Settings;
using Branta.Features.WalletVerification;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Branta.Features.WalletFocus;

public class FocusCommand : BaseCommand
{
    private readonly NotificationCenter _notificationCenter;
    private readonly SettingsStore _settings;
    private readonly LanguageStore _languageStore;

    private Dictionary<BaseWalletType, WalletStatus> _walletTypes;
    private readonly ILogger<FocusCommand> _logger;

    public FocusCommand(NotificationCenter notificationCenter, SettingsStore settings, LanguageStore languageStore,
        ILogger<FocusCommand> logger)
    {
        _notificationCenter = notificationCenter;
        _settings = settings;
        _languageStore = languageStore;
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
                Message = _languageStore.Format("FocusMessage", wallet.Name, wallet.Version,
                    wallet.Status.GetName(_languageStore))
            });

            _walletTypes[walletType] = wallet.Status;
        }
    }

    public void SetWallets(List<BaseWalletType> wallets)
    {
        _walletTypes = wallets.ToDictionary(w => w, _ => (WalletStatus)null);
    }
}
using Branta.Classes;
using Branta.Classes.Wallets;
using Branta.Enums;
using Branta.ViewModels;
using System.Diagnostics;

namespace Branta.Commands;

public class FocusCommand : BaseCommand
{
    private readonly WalletVerificationViewModel _viewModel;
    private readonly NotificationCenter _notificationCenter;
    private readonly Settings _settings;

    private Dictionary<BaseWallet, WalletStatus> _walletTypes;

    public FocusCommand(WalletVerificationViewModel viewModel, NotificationCenter notificationCenter, Settings settings)
    {
        _viewModel = viewModel;
        _notificationCenter = notificationCenter;
        _settings = settings;
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
                _walletTypes[walletType] = WalletStatus.None;
                continue;
            }

            var wallet = VerifyWalletsCommand.Verify(walletType);

            if (wallet.Status != _walletTypes.GetValueOrDefault(walletType))
            {
                _notificationCenter.Notify(new Notification
                {
                    Message = $"{wallet.Name} {wallet.Version} is running. Status: {wallet.Status.Name}"
                });

                _walletTypes[walletType] = wallet.Status;
            }
        }
    }
    
    public void SetWallets(List<BaseWallet> wallets)
    {
        _walletTypes = wallets.ToDictionary(w => w, _ => WalletStatus.None);
    }
}

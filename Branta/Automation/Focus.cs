using Branta.Automation.Wallets;
using Branta.Classes;
using Branta.Enums;
using System.Diagnostics;
using System.Windows.Forms;

namespace Branta.Automation;

public class Focus : BaseAutomation
{
    private Dictionary<BaseWallet, WalletStatus> _walletTypes;

    public Focus(NotifyIcon notifyIcon, Settings settings) : base(notifyIcon, settings, new TimeSpan(0, 0, 2))
    {
    }

    public override void Run()
    {
        if (!Settings.WalletVerification.LaunchingWalletEnabled || _walletTypes == null)
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

            var wallet = VerifyWallets.Verify(walletType);

            if (wallet.Status != _walletTypes.GetValueOrDefault(walletType))
            {
                NotifyIcon.ShowBalloonTip(new Notification
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
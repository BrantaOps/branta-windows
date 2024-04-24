using Branta.Automation.Wallets;
using Branta.Classes;
using Branta.Enums;
using System.Diagnostics;
using System.Windows.Forms;

namespace Branta.Automation;

public class Focus : BaseAutomation
{
    private readonly Dictionary<BaseWallet, WalletStatus> _walletTypes;

    public Focus(NotifyIcon notifyIcon, Settings settings) : base(notifyIcon, settings, new TimeSpan(0, 0, 2))
    {
        _walletTypes = BaseWallet.GetSupportedWallets().ToDictionary(w => w, _ => WalletStatus.None);
    }

    public override void Run()
    {
        if (!Settings.WalletVerification.LaunchingWalletEnabled)
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
}

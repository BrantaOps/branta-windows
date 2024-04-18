using Branta.Automation.Wallets;
using Branta.Classes;

namespace Branta.Automation;

public class LoadCheckSums : BaseAutomation
{
    public List<BaseWallet> WalletTypes = new();

    private readonly BrantaClient _brantaClient;

    public LoadCheckSums() : base(null, null, 60 * 60 * 4)
    {
        _brantaClient = new BrantaClient();
    }

    public override void Run()
    {
        Task.Run(LoadAsync);
    }

    public async Task LoadAsync()
    {
        var checkSums = await _brantaClient.GetCheckSumsAsync() ?? YamlLoader.LoadCheckSums();

        WalletTypes = BaseWallet.GetSupportedWallets(checkSums);
    }
}
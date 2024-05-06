using Branta.Automation.Wallets;
using Branta.Classes;

namespace Branta.Automation;

public class LoadCheckSums() : BaseAutomation(null, null, new TimeSpan(0, 30, 0))
{
    public List<BaseWallet> WalletTypes = new();

    private readonly BrantaClient _brantaClient = new();

    private const string CheckSumsPath = "CheckSums.yaml";

    public override void Run()
    {
        Task.Run(LoadAsync);
    }

    public async Task LoadAsync()
    {
        var checkSums = await LoadCheckSumsAsync();

        WalletTypes = BaseWallet.GetSupportedWallets(checkSums);
    }

    private async Task<CheckSums> LoadCheckSumsAsync()
    {
        var serverCheckSums = await _brantaClient.GetCheckSumsAsync();

        if (serverCheckSums != null)
        {
            FileStorage.Save(CheckSumsPath, serverCheckSums);

            return YamlLoader.Load<CheckSums>(serverCheckSums);
        }

        var cacheCheckSums = FileStorage.Load(CheckSumsPath);

        if (cacheCheckSums != null)
        {
            return YamlLoader.Load<CheckSums>(cacheCheckSums);
        }

        return YamlLoader.LoadCheckSums();
    }
}
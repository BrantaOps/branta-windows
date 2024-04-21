using Branta.Automation.Wallets;
using Branta.Classes;

namespace Branta.Automation;

public class LoadCheckSums : BaseAutomation
{
    public List<BaseWallet> WalletTypes = new();

    private readonly BrantaClient _brantaClient;

    private const string CheckSumsPath = "CheckSums.yaml";

    public LoadCheckSums() : base(null, null, 60 * 30)
    {
        _brantaClient = new BrantaClient();
    }

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
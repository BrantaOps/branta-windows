using Branta.Classes;
using Branta.Classes.Wallets;

namespace Branta.Stores;

public class CheckSumStore
{
    public List<BaseWallet> WalletTypes { get; private set; }
    public DateTime? LastUpdated { get; private set; }

    public Action<DateTime?> LastUpdatedEvent;

    private readonly BrantaClient _brantaClient;

    private const string CheckSumsPath = "CheckSums.yaml";

    public CheckSumStore()
    {
        _brantaClient = new BrantaClient();
    }

    public async Task LoadAsync()
    {
        var checkSums = await LoadHelperAsync();
        LastUpdated = DateTime.Now;
        LastUpdatedEvent?.Invoke(LastUpdated);

        WalletTypes = BaseWallet.GetWalletTypes(checkSums);
    }
    
    private async Task<CheckSums> LoadHelperAsync()
    {
        var serverCheckSums = await _brantaClient.GetCheckSumsAsync();

        if (serverCheckSums != null)
        {
            await FileStorage.SaveAsync(CheckSumsPath, serverCheckSums);

            return YamlLoader.Load<CheckSums>(serverCheckSums);
        }

        var cacheCheckSums = await FileStorage.LoadAsync(CheckSumsPath);

        if (cacheCheckSums != null)
        {
            return YamlLoader.Load<CheckSums>(cacheCheckSums);
        }

        return YamlLoader.LoadCheckSums();
    }
}

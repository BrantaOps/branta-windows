using Branta.Classes;

namespace Branta.Stores;

public class InstallerHashStore
{
    public Dictionary<string, string> InstallerHashes { get; private set; }
    public DateTime? LastUpdated { get; private set; }

    public Action<DateTime?> LastUpdatedEvent;

    private readonly BrantaClient _brantaClient;

    private const string InstallerHashPath = "InstallerHash.yaml";

    public InstallerHashStore()
    {
        _brantaClient = new BrantaClient();
    }

    public async Task LoadAsync()
    {
        InstallerHashes = await LoadHelperAsync();

        LastUpdated = DateTime.Now;
        LastUpdatedEvent?.Invoke(LastUpdated);
    }

    public async Task<Dictionary<string, string>> LoadHelperAsync()
    {
        var serverHashes = await _brantaClient.GetInstallerHashesAsync();

        if (serverHashes != null)
        {
            await FileStorage.SaveAsync(InstallerHashPath, serverHashes);

            return YamlLoader.Load<Dictionary<string, string>>(serverHashes);
        }

        var cacheHashes = await FileStorage.LoadAsync(InstallerHashPath);

        if (cacheHashes != null)
        {
            return YamlLoader.Load<Dictionary<string, string>>(cacheHashes);
        }

        return YamlLoader.LoadInstallerHashes();
    }
}
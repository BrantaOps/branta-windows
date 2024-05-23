using Branta.Classes;
using Branta.ViewModels;

namespace Branta.Commands;

public class LoadInstallerHashesCommand : BaseAsyncCommand
{
    private readonly BrantaClient _brantaClient = new();

    private const string InstallerHashPath = "InstallerHash.yaml";

    public override async Task ExecuteAsync(object parameter)
    {
        var viewModel = (InstallerVerificationViewModel)parameter;

        var installerHashes = await LoadInstallerHashesAsync();

        viewModel.SetInstallerHashes(installerHashes);
        viewModel.IsLoading = false;
    }

    public async Task<Dictionary<string, string>> LoadInstallerHashesAsync()
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
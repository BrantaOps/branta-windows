using Branta.Classes;
using Branta.Classes.Wallets;
using Branta.ViewModels;

namespace Branta.Commands;

public class LoadCheckSumsCommand : BaseAsyncCommand
{
    private readonly BrantaClient _brantaClient = new BrantaClient();
    private readonly WalletVerificationViewModel _viewModel;

    private const string CheckSumsPath = "CheckSums.yaml";

    public LoadCheckSumsCommand(WalletVerificationViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        var checkSums = await LoadCheckSumsAsync();

        _viewModel.WalletTypes = BaseWallet.GetSupportedWallets(checkSums);
        _viewModel.IsLoading = false;
    }

    private async Task<CheckSums> LoadCheckSumsAsync()
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

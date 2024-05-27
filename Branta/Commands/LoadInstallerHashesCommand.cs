using Branta.Classes;
using Branta.Stores;
using Branta.ViewModels;

namespace Branta.Commands;

public class LoadInstallerHashesCommand : BaseAsyncCommand
{
    private readonly InstallerHashStore _installerHashStore;
    private readonly BrantaClient _brantaClient = new();

    public LoadInstallerHashesCommand(InstallerHashStore installerHashStore)
    {
        _installerHashStore = installerHashStore;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        var viewModel = (InstallerVerificationViewModel)parameter;

        await _installerHashStore.LoadAsync();

        viewModel.SetInstallerHashes(_installerHashStore.InstallerHashes);
        viewModel.IsLoading = false;
    }
}
using Branta.Stores;
using Branta.ViewModels;

namespace Branta.Commands;

public class LoadCheckSumsCommand : BaseAsyncCommand
{
    private readonly CheckSumStore _checkSumStore;

    public LoadCheckSumsCommand(CheckSumStore checkSumStore)
    {
        _checkSumStore = checkSumStore;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        var viewModel = (WalletVerificationViewModel)parameter;

        await _checkSumStore.LoadAsync();

        viewModel.IsLoading = false;
        viewModel.WalletTypes = _checkSumStore.WalletTypes;
    }
}
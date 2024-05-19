using Branta.Stores;
using Branta.ViewModels;

namespace Branta.Commands;

public class LoadCheckSumsCommand : BaseAsyncCommand
{
    private readonly WalletVerificationViewModel _viewModel;
    private readonly CheckSumStore _checkSumStore;

    public LoadCheckSumsCommand(WalletVerificationViewModel viewModel, CheckSumStore checkSumStore)
    {
        _viewModel = viewModel;
        _checkSumStore = checkSumStore;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        await _checkSumStore.LoadAsync();

        _viewModel.IsLoading = false;
        _viewModel.WalletTypes = _checkSumStore.WalletTypes;
    }
}
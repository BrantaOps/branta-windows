using System.Windows;
using System.Windows.Controls;
using Branta.Classes;
using Branta.Stores;
using Branta.ViewModels;

namespace Branta.Windows;

public partial class SettingsWindow
{
    private readonly SettingsViewModel _viewModel;

    public SettingsWindow(Settings settings, CheckSumStore checkSumStore, InstallerHashStore installerHashStore, WalletVerificationViewModel walletVerificationViewModel, InstallerVerificationViewModel installerVerificationViewModel)
    {
        InitializeComponent();

        _viewModel = new SettingsViewModel(settings, checkSumStore, installerHashStore, walletVerificationViewModel, installerVerificationViewModel);
        DataContext = _viewModel;

        SetLanguageDictionary();

        var verifyEveryOptions = new List<TimeSpan>
        {
            new(0, 0, 1),
            new(0, 0, 5),
            new(0, 0, 10),
            new(0, 0, 30),
            new(0, 1, 0),
            new(0, 5, 0),
            new(0, 10, 0),
            new(0, 30, 0)
        };

        foreach (var option in verifyEveryOptions)
        {
            ComboBoxVerifyEvery.Items.Add(new ComboBoxItem
            {
                IsSelected = option == settings.WalletVerification.WalletVerifyEvery,
                Content = option.Format(),
                Tag = option
            });
        }
    }

    private void ComboBoxVerifyEvery_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = (ComboBox)sender;
        var comboBoxItem = (ComboBoxItem)comboBox.SelectedValue;

        _viewModel.VerifyEvery = (TimeSpan)comboBoxItem.Tag;
    }
    
    public Settings GetSettings()
    {
        return _viewModel.GetSettings();
    }

    private void OnClick_Refresh(object sender, RoutedEventArgs e)
    {
        _viewModel.LoadCheckSumsCommand.Execute(_viewModel.WalletVerificationViewModel);
        _viewModel.LoadInstallerHashesCommand.Execute(_viewModel.InstallerVerificationViewModel);
    }
}
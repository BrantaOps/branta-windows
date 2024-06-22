using Branta.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Branta.Views;

public partial class SettingsView : UserControl
{
	public SettingsView()
	{
		InitializeComponent();
	}

	private void OnClick_Refresh(object sender, RoutedEventArgs e)
	{
		var viewModel = (SettingsViewModel)DataContext;

		viewModel.LoadCheckSumsCommand.Execute(viewModel.WalletVerificationViewModel);
		viewModel.LoadInstallerHashesCommand.Execute(viewModel.InstallerVerificationViewModel);
	}
}

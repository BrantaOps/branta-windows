using Branta.ViewModels;

namespace Branta.Commands;

public class SettingsSaveCommand : BaseCommand
{
	public override void Execute(object parameter)
	{
		var settingsViewModel = (SettingsViewModel)parameter;

		var settings = settingsViewModel.GetSettings();

		if (settings.WalletVerification.WalletVerifyEvery != settingsViewModel.Settings.WalletVerification.WalletVerifyEvery)
		{
			settingsViewModel.WalletVerificationViewModel.SetTimer(settings.WalletVerification.WalletVerifyEvery);
		}

		settingsViewModel.Settings.Update(settings);
	}
}

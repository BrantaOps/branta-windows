using Branta.Classes;
using Branta.Commands;
using Branta.Stores;
using Branta.ViewModels;
using System.Windows.Input;

namespace Branta.Windows;

public partial class SettingsWindow
{
    public readonly SettingsViewModel SettingsViewModel;

    public ICommand HelpCommand { get; }

    public SettingsWindow(LanguageStore languageStore, SettingsViewModel settingsViewModel, ExtendedKeyManagerViewModel extendedKeyManagerViewModel)
    {
        InitializeComponent();
        DataContext = this;

        this.SetLanguageDictionary(languageStore);

        SettingsView.DataContext = settingsViewModel;
        ExtendedKeyManagerView.DataContext = extendedKeyManagerViewModel;
        HelpCommand = new HelpCommand();
    }
}
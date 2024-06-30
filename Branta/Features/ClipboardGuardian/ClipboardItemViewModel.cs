using Branta.Features.Main;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Branta.Features.ClipboardGuardian;

public class ClipboardItemViewModel : ObservableObject
{
    private readonly ClipboardItem _clipboardItem;
    private readonly LanguageStore _languageStore;

    public string DisplayName => _languageStore.Get("Clipboard") +
                                 (!string.IsNullOrEmpty(_clipboardItem.Name) ? $": {_clipboardItem.Name}" : null);

    public string Name => _clipboardItem.Name;

    public string Value => _clipboardItem.Value;

    public bool IsDefault => _clipboardItem.IsDefault;

    public ClipboardItemViewModel(ClipboardItem clipboardItem, LanguageStore languageStore)
    {
        _clipboardItem = clipboardItem;
        _languageStore = languageStore;
    }
}
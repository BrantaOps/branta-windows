using Branta.Models;
using System.Windows;

namespace Branta.ViewModels;

public class ClipboardItemViewModel : BaseViewModel
{
    private readonly ClipboardItem _clipboardItem;
    private readonly ResourceDictionary _resourceDictionary;

    public string DisplayName => _resourceDictionary["Clipboard"] +
                                 (!string.IsNullOrEmpty(_clipboardItem.Name) ? $": {_clipboardItem.Name}" : null);

    public string Name => _clipboardItem.Name;

    public string Value => _clipboardItem.Value;

    public bool IsDefault => _clipboardItem.IsDefault;

    public ClipboardItemViewModel(ClipboardItem clipboardItem, ResourceDictionary resourceDictionary)
    {
        _clipboardItem = clipboardItem;
        _resourceDictionary = resourceDictionary;
    }
}
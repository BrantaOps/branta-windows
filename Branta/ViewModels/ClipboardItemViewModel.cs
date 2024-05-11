using Branta.Models;

namespace Branta.ViewModels;

public class ClipboardItemViewModel : BaseViewModel
{
    private readonly ClipboardItem _clipboardItem;

    public string Name => _clipboardItem.Name;

    public string Value => _clipboardItem.Value;

    public ClipboardItemViewModel(ClipboardItem clipboardItem)
    {
        _clipboardItem = clipboardItem;
    }
}

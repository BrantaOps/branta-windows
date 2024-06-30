using Branta.Classes;
using Branta.Features.Main;

namespace Branta.Features.ClipboardGuardian;

public class ClipboardItem
{
    public string Name { get; set; }

    public string Value { get; set; }

    public bool IsDefault { get; set; }

    public Notification Notification { get; set; }

    public ClipboardItem()
    {
    }

    public ClipboardItem(bool enabled, LanguageStore languageStore, string resourceKey, string value = null)
    {
        Name = languageStore.Get($"ClipboardGuardian_{resourceKey}Name");
        Value = value;
        IsDefault = false;
        Notification = enabled
            ? new Notification
            {
                Title = languageStore.Get($"ClipboardGuardian_{resourceKey}Title"),
                Message = languageStore.Get($"ClipboardGuardian_{resourceKey}Message"),
            }
            : null;
    }
}
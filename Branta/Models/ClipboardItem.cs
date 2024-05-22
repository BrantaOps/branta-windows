using Branta.Classes;
using System.Windows;

namespace Branta.Models;

public class ClipboardItem
{
    public string Name { get; set; }

    public string Value { get; set; }

    public bool IsDefault { get; set; }

    public Notification Notification { get; set; }

    public ClipboardItem()
    {
    }

    public ClipboardItem(bool enabled, ResourceDictionary resourceDictionary, string resourceKey, string value = null)
    {
        Name = resourceDictionary[$"ClipboardGuardian_{resourceKey}Name"]?.ToString();
        Value = value;
        IsDefault = false;
        Notification = enabled ? new Notification()
        {
            Title = resourceDictionary[$"ClipboardGuardian_{resourceKey}Title"]?.ToString(),
            Message = resourceDictionary[$"ClipboardGuardian_{resourceKey}Message"]?.ToString(),
        } : null;
    }
}

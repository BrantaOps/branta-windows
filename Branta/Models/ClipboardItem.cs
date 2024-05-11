using Branta.Classes;

namespace Branta.Models;

public class ClipboardItem
{
    public string Name { get; set; }

    public string Value { get; set; }

    public Notification Notification { get; set; }
}

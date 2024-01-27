using Microsoft.Win32;
using System.Diagnostics;

namespace Branta.Classes;

public class RegistryHelper
{
    public static string GetValue(string subKeyName, string valueName)
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(subKeyName);

            var value = key?.GetValue(valueName);

            if (value != null)
            {
                return value.ToString();
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Error accessing registry: {ex.Message}");
        }

        return null;
    }

    public static string FindDisplayName(string uninstallKeyPath, string softwareName)
    {
        try
        {
            using var uninstallKey = Registry.LocalMachine.OpenSubKey(uninstallKeyPath);
            if (uninstallKey != null)
            {
                foreach (var subKeyName in uninstallKey.GetSubKeyNames())
                {
                    using var subKey = uninstallKey.OpenSubKey(subKeyName);
                    var displayNameValue = subKey?.GetValue("DisplayName");

                    if (displayNameValue?.ToString() != null && displayNameValue.ToString()!.Contains(softwareName))
                    {
                        return subKey.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Error accessing registry: {ex.Message}");
        }

        return null;
    }
}

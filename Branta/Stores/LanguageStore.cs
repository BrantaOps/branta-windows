using System.Windows;

namespace Branta.Stores;

public class LanguageStore
{
    public ResourceDictionary ResourceDictionary { get; set; }

    public LanguageStore(ResourceDictionary resourceDictionary)
    {
        ResourceDictionary = resourceDictionary;
    }

    public string Get(string key)
    {
        return ResourceDictionary[key]?.ToString();
    }

    public string Format(string key, params object[] values)
    {
        return string.Format(Get(key), values);
    }

    public static LanguageStore Load()
    {
        var resourceDictionary = GetResourceDictionary();

        return new LanguageStore(resourceDictionary);
    }

    private static ResourceDictionary GetResourceDictionary()
    {
        try
        {
            var resourceDictionary = new ResourceDictionary
            {
                Source = Thread.CurrentThread.CurrentCulture.ToString() switch
                {
                    "en-US" => new Uri("Resources\\StringResources.xaml", UriKind.Relative),
                    _ => new Uri("Resources\\StringResources.xaml", UriKind.Relative),
                }
            };

            return resourceDictionary;
        }
        catch
        {
            return null;
        }
    }
}
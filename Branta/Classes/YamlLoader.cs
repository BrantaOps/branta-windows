using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Branta.Classes;

public class YamlLoader
{
    public static Dictionary<string, string> LoadInstallerHashes()
    {
        var yaml = File.ReadAllText("Assets\\InstallerHashes.yaml");

        return Load<Dictionary<string, string>>(yaml);
    }

    public static CheckSums LoadCheckSums()
    {
        var yaml = File.ReadAllText("Assets\\CheckSums.yaml");

        return Load<CheckSums>(yaml);
    }

    public static T Load<T>(string yaml)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        return deserializer.Deserialize<T>(yaml);
    }
}
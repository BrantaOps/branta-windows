using System.IO;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Branta.Classes;

public class YamlLoader
{
    public static Dictionary<string, string> LoadInstallerHashes()
    {
        using var reader = new StreamReader("Assets\\InstallerHashes.yaml");

        return LoadInstallerHashes(reader);
    }

    public static Dictionary<string, string> LoadInstallerHashes(StreamReader streamReader)
    {
        var result = new Dictionary<string, string>();

        var yaml = new YamlStream();
        yaml.Load(streamReader);

        foreach (var doc in yaml.Documents)
        {
            var mappingNode = (YamlMappingNode)doc.RootNode;

            foreach (var entry in mappingNode.Children)
            {
                var hash = ((YamlScalarNode)entry.Key).Value;
                var filename = ((YamlScalarNode)entry.Value).Value;

                result.Add(hash, filename);
            }
        }

        return result;
    }

    public static CheckSums LoadCheckSums()
    {
        var yaml = File.ReadAllText("Assets\\CheckSums.yaml");

        return LoadCheckSums(yaml);
    }

    public static CheckSums LoadCheckSums(string yaml)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        return deserializer.Deserialize<CheckSums>(yaml);
    }
}
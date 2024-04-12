using System.IO;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Branta.Classes;

public class YamlLoader
{
    public static Dictionary<string, string> LoadInstallerHashes()
    {
        var result = new Dictionary<string, string>();

        using var reader = new StreamReader("Assets\\InstallerHashes.yaml");
        var yaml = new YamlStream();
        yaml.Load(reader);

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
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var yaml = File.ReadAllText("Assets\\CheckSums.yaml");

        return deserializer.Deserialize<CheckSums>(yaml);
    }
}
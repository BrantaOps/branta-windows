using System.IO;
using YamlDotNet.RepresentationModel;

namespace Branta.Classes;

public class YamlLoader
{
    public static Dictionary<string, string> LoadYamlFile(string filePath)
    {
        var result = new Dictionary<string, string>();

        using var reader = new StreamReader(filePath);
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
}
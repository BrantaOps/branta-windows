using System.IO;

namespace Branta.Classes;

public static class FileStorage
{
    public static async Task SaveAsync(string path, string content)
    {
        try
        {
            var fullPath = GetBrantaDataPath(path);

            await File.WriteAllTextAsync(fullPath, content);
        }
        catch
        {
            // ignored
        }
    }

    public static async Task<string> LoadAsync(string path)
    {
        var fullPath = GetBrantaDataPath(path);

        if (!File.Exists(fullPath))
        {
            return null;
        }

        return await File.ReadAllTextAsync(fullPath);
    }

    public static string GetBrantaDataPath(params string[] path)
    {
        var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Branta", Path.Combine(path));

        var file = new FileInfo(folder);
        file.Directory?.Create();

        return folder;
    }
}
using System.IO;

namespace Branta.Classes;

public static class FileStorage
{
    public static async Task SaveAsync(string path, string content)
    {
        try
        {
            var file = new FileInfo(GetBrantaDataPath(path));
            file.Directory?.Create();
            await File.WriteAllTextAsync(GetBrantaDataPath(path), content);
        }
        catch
        {
        }
    }

    public static async Task<string> LoadAsync(string path)
    {
        path = GetBrantaDataPath(path);

        if (!File.Exists(path))
        {
            return null;
        }

        return await File.ReadAllTextAsync(path);
    }

    public static string GetBrantaDataPath(params string[] path)
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Branta", Path.Combine(path));
    }
}
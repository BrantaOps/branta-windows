using System.IO;

namespace Branta.Classes;

public static class FileStorage
{
    public static async Task SaveAsync(string path, string content)
    {
        try
        {
            await File.WriteAllTextAsync(GetFullPath(path), content);
        }
        catch
        {
        }
    }

    public static async Task<string> LoadAsync(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        return await File.ReadAllTextAsync(GetFullPath(path));
    }

    private static string GetFullPath(string path)
    {
        return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Branta", path);
    }
}
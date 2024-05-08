using System.IO;

namespace Branta.Classes;

public static class FileStorage
{
    public static void Save(string path, string content)
    {
        var file = new FileInfo(GetFullPath(path));
        file.Directory?.Create();
        File.WriteAllText(file.FullName, content);
    }

    public static string Load(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        return File.ReadAllText(GetFullPath(path));
    }

    private static string GetFullPath(string path)
    {
        return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Branta", path);
    }
}
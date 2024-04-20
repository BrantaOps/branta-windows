using System.IO;

namespace Branta.Classes;

public static class FileStorage
{
    public static void Save(string path, string content)
    {
        File.WriteAllText(GetFullPath(path), content);
    }

    public static string Load(string path)
    {
        return File.ReadAllText(GetFullPath(path));
    }

    private static string GetFullPath(string path)
    {
        return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Branta", path);
    }
}
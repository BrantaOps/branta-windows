using System.IO;
using System.Net.Http;

namespace Branta.Classes;

internal class BrantaClient
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://api.branta.pro")
    };

    public async Task<Dictionary<string, string>> GetInstallerHashesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/installer_hashes");
            var content = await response.Content.ReadAsStreamAsync();

            return YamlLoader.LoadInstallerHashes(new StreamReader(content));
        }
        catch
        {
            return null;
        }
    }
}
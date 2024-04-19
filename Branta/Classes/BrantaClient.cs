using System.IO;
using System.Net.Http;

namespace Branta.Classes;

public class BrantaClient
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://api.branta.pro/v1/")
    };

    public async Task<Dictionary<string, string>> GetInstallerHashesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("installer_hashes");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStreamAsync();

            return YamlLoader.LoadInstallerHashes(new StreamReader(content));
        }
        catch
        {
            return null;
        }
    }

    public async Task<CheckSums> GetCheckSumsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("checksums?platform=windows");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            return YamlLoader.LoadCheckSums(content);
        }
        catch
        {
            return null;
        }
    }
}
using System.Net.Http;

namespace Branta.Classes;

public class BrantaClient
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://api.branta.pro/v1/")
    };

    public async Task<string> GetInstallerHashesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("installer_hashes");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }
        catch
        {
            return null;
        }
    }

    public async Task<string> GetCheckSumsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("checksums?platform=windows");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }
        catch
        {
            return null;
        }
    }
}
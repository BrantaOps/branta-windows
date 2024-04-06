using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Branta.Classes;

public class GitHubRelease
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class GitHubClient
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://api.github.com/")
    };

    private const string Owner = "BrantaOps";
    private const string Repo = "branta-windows";

    public async Task<string> GetLatestReleaseVersionAsync()
    {
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("request");
        var response = await _httpClient.GetAsync($"/repos/{Owner}/{Repo}/releases/latest");
        var temp = await response.Content.ReadAsStringAsync();

        var githubRelease = JsonSerializer.Deserialize<GitHubRelease>(temp);

        return githubRelease.Name;
    }
}

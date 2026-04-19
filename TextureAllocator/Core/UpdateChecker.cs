using NuGet.Versioning;
using System.Text.Json;
using TextureAllocator.Properties;

namespace TextureAllocator.Core;

internal static class UpdateChecker
{
    static HttpClient httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(5),
        DefaultRequestHeaders =
        {
            { "User-Agent", "Chrome" }
        }
    };
    internal static async Task<UpdateCheckResult> AutoCheckForUpdate()
    {
        var currentTime = DateTime.Now;
        var lastCheckDate = Settings.Default.LastUpdateCheckDate;
        var result = new UpdateCheckResult(false, "","");
        if (currentTime - lastCheckDate > TimeSpan.FromHours(0))
        {
            result = await CheckForUpdates().ConfigureAwait(false);
        }
        return result;
    }
    internal static async Task<UpdateCheckResult> CheckForUpdates()
    {

        // GitHub API で最新リリース情報を取得
        string apiUrl = "https://api.github.com/repos/Whiter002/3DModels_UVTextureAllocator/releases/latest";

        UpdateCheckResult result = new(false, "","");

        // リリース情報のJSONを取得
        string releaseJson = "";
        var responseMessage = await httpClient.GetAsync(apiUrl).ConfigureAwait(false);
        if (!responseMessage.IsSuccessStatusCode) return result;
        releaseJson = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        var utf8Reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(releaseJson));

        // assets 配列から LatestVersion.json のダウンロードURLを探す
        if (!JsonDocument.TryParseValue(ref utf8Reader, out var doc)) return result;

        bool? updateAvailable = null ;
        {//Compare Versions
            NuGetVersion.TryParse(VersionInfo.CurrentVersion, out var currentVersion);
            string latestVersionStr = doc.RootElement.GetProperty("tag_name").GetString() ?? "";
            NuGetVersion.TryParse(latestVersionStr, out var latestVersion);
            if (latestVersion > currentVersion) updateAvailable = true;
        }

        if (!doc.RootElement.TryGetProperty("body", out var releaseNoteElement)) return result;
        var releaseNotes = releaseNoteElement.GetString();
        
        string zipDownloadUrl = null;
        if (updateAvailable ?? false) {
            if (!doc.RootElement.TryGetProperty("assets", out var assets)) return result;
            foreach (var asset in assets.EnumerateArray())
            {
                if (!asset.TryGetProperty("name", out var fileElement) || !String.Equals(fileElement.GetString(), "release.zip")) continue;
                zipDownloadUrl = asset.GetProperty("browser_download_url").GetString()!;
                break;
            }
        }

        Settings.Default.LastUpdateCheckDate = DateTime.Now;
        Settings.Default.Save();
        result = new(updateAvailable?? false, zipDownloadUrl ?? "",releaseNotes ?? "");
        return result;
    }

}

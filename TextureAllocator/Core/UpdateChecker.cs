using NuGet.Versioning;
using System.Text.Json;
using TextureAllocator.Properties;

namespace TextureAllocator.Core;

internal static class UpdateChecker
{
    internal static async Task<UpdateCheckResult> AutoCheckForUpdate()
    {
        var currentTime = DateTime.Now;
        var lastCheckDate = Settings.Default.LastUpdateCheckDate;
        var result = new UpdateCheckResult(false, "");
        {
            result = await CheckForUpdates();
        }
        return result;
    }


    internal static async Task<UpdateCheckResult> CheckForUpdates()
    {

        // GitHub API で最新リリース情報を取得
        string apiUrl = "https://api.github.com/repos/Whiter002/3DModels_UVTextureAllocator/releases/latest";

        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("User-Agent", "Chrome");

        // リリース情報のJSONを取得
        string releaseJson = await client.GetStringAsync(apiUrl);

        // assets 配列から LatestVersion.json のダウンロードURLを探す
        using var doc = System.Text.Json.JsonDocument.Parse(releaseJson);
        var assets = doc.RootElement.GetProperty("assets");

        bool? updateAvailable = null ;
        string zipDownloadUrl = null;
        NuGetVersion.TryParse(VersionInfo.CurrentVersion, out var currentVersion);

        foreach (var asset in assets.EnumerateArray())
        {
            string assetName = asset.GetProperty("name").GetString();
            if(assetName == "release.zip")
            {
                zipDownloadUrl = asset.GetProperty("browser_download_url").GetString()!;
                if (updateAvailable is null) continue;
                break;
            }
            if (assetName == "LatestVersion.json")
            {
                string downloadUrl = asset.GetProperty("browser_download_url").GetString()!;
                string versionJson = await client.GetStringAsync(downloadUrl);
                var versionData = JsonSerializer.Deserialize<LatestVersionData>(versionJson);
                NuGetVersion.TryParse(versionData.LatestVersion, out var latestVersion);
                if (latestVersion > currentVersion)updateAvailable = true;
                if (zipDownloadUrl is null) continue;
                break;
            }
        }

        Settings.Default.LastUpdateCheckDate = DateTime.Now;
        return new(updateAvailable??false, zipDownloadUrl ?? "");
    }

}

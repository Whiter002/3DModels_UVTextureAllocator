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
    // FIXME: ネットワーク障害時に TaskCanceledException / HttpRequestException が未処理のまま伝播する。try-catch を追加するか呼び出し側でハンドリングすること。
    internal static async Task<UpdateCheckResult> CheckForUpdates()
    {

        // GitHub API で最新リリース情報を取得
        string apiUrl = "https://api.github.com/repos/Whiter002/3DModels_UVTextureAllocator/releases/latest";

        UpdateCheckResult result = new(false, "","");

        // リリース情報のJSONを取得
        string releaseJson = "";
        // FIXME: responseMessage が Dispose されていない。using を追加すること。
        var responseMessage = await httpClient.GetAsync(apiUrl).ConfigureAwait(false);
        if (!responseMessage.IsSuccessStatusCode) return result;
        releaseJson = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        var utf8Reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(releaseJson));

        if (!JsonDocument.TryParseValue(ref utf8Reader, out var doc)) return result;

        bool? updateAvailable = null ;
        {//Compare Versions
            NuGetVersion.TryParse(VersionInfo.CurrentVersion, out var currentVersion);
            // FIXME: tag_name が存在しない場合 GetProperty は KeyNotFoundException をスローする。TryGetProperty を使うこと。
            string latestVersionStr = doc.RootElement.GetProperty("tag_name").GetString() ?? "";
            NuGetVersion.TryParse(latestVersionStr, out var latestVersion);
            // FIXME: TryParse が失敗すると currentVersion / latestVersion が null になり、比較結果が常に false になる。パース失敗時のハンドリングを追加すること。
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

        // FIXME: JsonDocument (doc) が Dispose されていない。using を追加すること。
        Settings.Default.LastUpdateCheckDate = DateTime.Now;
        Settings.Default.Save();
        result = new(updateAvailable?? false, zipDownloadUrl ?? "",releaseNotes ?? "");
        return result;
    }

}

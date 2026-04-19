using System.Text.Json.Serialization;

namespace TextureAllocator.Core;

// FIXME: リファクタリング後 LatestVersionData は使用されていない可能性がある。参照がなければ削除すること。
public class LatestVersionData
{
    [JsonPropertyName("latestVersion")]
    public string LatestVersion { get; set; }
    [JsonPropertyName("hash")]
    public string SHA256 { get; set; }

}

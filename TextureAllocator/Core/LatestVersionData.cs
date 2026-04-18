using System.Text.Json.Serialization;

namespace TextureAllocator.Core;

public class LatestVersionData
{
    [JsonPropertyName("latestVersion")]
    public string LatestVersion { get; set; }
    [JsonPropertyName("hash")]
    public string SHA256 { get; set; }

}

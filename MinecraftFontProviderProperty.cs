using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace FontJsonGenerator;

[Serializable]
public class MinecraftFontProviderProperty
{
    [JsonProperty("type")]
    public string Type { get; set; } = ProviderType.NoType;

    [JsonProperty("file")]
    public string ResourcePath { get; set; } = string.Empty;

    [JsonProperty("ascent")]
    public int Ascent { get; set; } = -1;

    [JsonProperty("height")]
    public int? Height { get; set; }

    [JsonProperty("chars")]
    public string[] Charaters { get; set; } = Array.Empty<string>();

    [AllowNull]
    [JsonProperty("sizes", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Sizes { get; set; }

    [AllowNull]
    [JsonProperty("template", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Template { get; set; }

    public bool IsVaild()
    {
        return Type == ProviderType.NoType
               || ResourcePath == string.Empty
               || Ascent <= -1
               || Height <= -1
               || Charaters.Length == 0
               || Sizes == string.Empty
               || Template == string.Empty;
    }

    public override string ToString() => JsonConvert.SerializeObject(this);
}

public class ProviderType
{
    public static string Bitmap = "bitmap";

    public static string TrueTypeFont = "ttf";

    public static string LegacyUnicode = "legacy_unicode";

    public static string NoType = string.Empty;
}
using Newtonsoft.Json;

namespace FontJsonGenerator.GenerateConfig;

[Serializable]
public class GlyphGenConfig
{
    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    /// <summary>
    /// 单张tile的大小
    /// </summary>
    [JsonProperty("tileSize")]
    public int TileSize { get; set; }

    [JsonProperty("spacing_x")]
    public int HorizonalSpacing { get; set; }

    [JsonProperty("spacing_y")]
    public int VerticalSpacing { get; set; }

    [JsonProperty("from")]
    public int StartAt { get; set; }

    [JsonProperty("to")]
    public int EndsAt { get; set; }
    
    [JsonProperty("pack_path")]
    public string BasePath { get; set; }
    
    [JsonProperty("save_step")]
    public bool SaveSteps { get; set; }
    
    [JsonProperty("fallback")]
    public string? FallbackTexturePath { get; set; }
}
using Newtonsoft.Json;

namespace FontJsonGenerator.GenerateConfig;

[Serializable]
public class GlyphGenConfig
{
    /// <summary>
    /// 是否启用Glyph生成
    /// </summary>
    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    /// <summary>
    /// 单张tile的大小
    /// </summary>
    [JsonProperty("tileSize")]
    public int TileSize { get; set; }

    /// <summary>
    /// 水平间距
    /// </summary>
    [JsonProperty("spacing_x")]
    public int HorizonalSpacing { get; set; }

    /// <summary>
    /// 竖直间距
    /// </summary>
    [JsonProperty("spacing_y")]
    public int VerticalSpacing { get; set; }

    /// <summary>
    /// 起始字符
    /// </summary>
    [JsonProperty("from")]
    public int StartAt { get; set; }

    /// <summary>
    /// 终止字符
    /// </summary>
    [JsonProperty("to")]
    public int EndsAt { get; set; }

    /// <summary>
    /// 资源包文件路径
    /// </summary>
    [JsonProperty("pack_path")]
    public string BasePath { get; set; }

    /// <summary>
    /// 保存每一步生成的图像
    /// </summary>
    [JsonProperty("save_step")]
    public bool SaveSteps { get; set; }

    /// <summary>
    /// 后备纹理路径
    /// </summary>
    [JsonProperty("fallback")]
    public string? FallbackTexturePath { get; set; }
}